using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.ECS.Components.Physics.Colliders;
using ProjectGaem2.Engine.Utils.Extensions;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.Physics
{
    public class SpatialHash
    {
        SpatialHashDictionary _cellStore = new();
        int _cellSize;
        float _inverseCellSize;
        public Rectangle GridBounds = new();
        HashSet<Collider> _aabbBufferHashSet = [];

        public SpatialHash(int cellSize = 100)
        {
            _cellSize = cellSize;
            _inverseCellSize = 1f / _cellSize;
        }

        Point CellCoords(int x, int y)
        {
            return new Point(
                (int)MathF.Floor(x * _inverseCellSize),
                (int)MathF.Floor(y * _inverseCellSize)
            );
        }

        Point CellCoords(float x, float y)
        {
            return new Point(
                (int)MathF.Floor(x * _inverseCellSize),
                (int)MathF.Floor(y * _inverseCellSize)
            );
        }

        List<Collider> CellAtPosition(int x, int y)
        {
            if (!_cellStore.TryGetValue(x, y, out List<Collider> cell))
            {
                cell = [];
                _cellStore.Add(x, y, cell);
            }
            return cell;
        }

        public void Register(Collider collider)
        {
            var bounds = collider.Bounds;
            collider.RegisteredBounds = bounds;

            var p1 = CellCoords(bounds.X, bounds.Y);
            var p2 = CellCoords(bounds.Right, bounds.Bottom);

            if (!GridBounds.Contains(p1))
            {
                RectangleExt.Union(ref GridBounds, ref p1, out GridBounds);
            }

            if (!GridBounds.Contains(p2))
            {
                RectangleExt.Union(ref GridBounds, ref p2, out GridBounds);
            }

            for (var x = p1.X; x <= p2.X; x++)
            {
                for (var y = p1.Y; y <= p2.Y; y++)
                {
                    var c = CellAtPosition(x, y);
                    c.Add(collider);
                }
            }
        }

        public void Unregister(Collider collider)
        {
            var bounds = collider.RegisteredBounds;
            var p1 = CellCoords(bounds.X, bounds.Y);
            var p2 = CellCoords(bounds.Right, bounds.Bottom);

            for (var x = p1.X; x <= p2.X; x++)
            {
                for (var y = p1.Y; y <= p2.Y; y++)
                {
                    var c = CellAtPosition(x, y);
                    c?.Remove(collider);
                }
            }
        }

        public void Clear() => _cellStore.Clear();

        public HashSet<Collider> GetAll() => _cellStore.GetAll();

        public HashSet<Collider> Aabb(in RectangleF bounds, Collider excludeCollider)
        {
            _aabbBufferHashSet.Clear();

            var p1 = CellCoords(bounds.X, bounds.Y);
            var p2 = CellCoords(bounds.Right, bounds.Bottom);

            for (var x = p1.X; x <= p2.X; x++)
            {
                for (var y = p1.Y; y <= p2.Y; y++)
                {
                    var cell = CellAtPosition(x, y);
                    if (cell is null)
                    {
                        continue;
                    }

                    for (var i = 0; i < cell.Count; i++)
                    {
                        var collider = cell[i];

                        if (collider == excludeCollider)
                        {
                            continue;
                        }

                        if (bounds.Intersects(collider.Bounds))
                        {
                            _aabbBufferHashSet.Add(collider);
                        }
                    }
                }
            }

            return _aabbBufferHashSet;
        }
    }

    class SpatialHashDictionary
    {
        Dictionary<long, List<Collider>> _store = [];

        long HashKey(int x, int y)
        {
            return (long)x << 32 | (uint)y;
        }

        public void Add(int x, int y, List<Collider> colliders) =>
            _store.Add(HashKey(x, y), colliders);

        public void Remove(Collider collider)
        {
            foreach (var colliders in _store.Values)
            {
                colliders.Remove(collider);
            }
        }

        public bool TryGetValue(int x, int y, out List<Collider> colliders) =>
            _store.TryGetValue(HashKey(x, y), out colliders);

        public HashSet<Collider> GetAll()
        {
            var set = new HashSet<Collider>();

            foreach (var colliders in _store.Values)
            {
                set.UnionWith(colliders);
            }

            return set;
        }

        public void Clear() => _store.Clear();
    }
}
