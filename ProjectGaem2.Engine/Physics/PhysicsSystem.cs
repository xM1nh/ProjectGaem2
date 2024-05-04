using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.ECS.Components.Physics.Colliders;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.Physics
{
    public static class PhysicsSystem
    {
        static SpatialHash _spatialHash;
        public static int SpatialHashCellSize { get; set; } = 100;
        public static Vector2 Gravity { get; set; } = new(0, 9.81f);

        public static void Reset()
        {
            _spatialHash = new SpatialHash(SpatialHashCellSize);
        }

        public static void Clear() => _spatialHash.Clear();

        public static void AddCollider(Collider collider) => _spatialHash.Register(collider);

        public static void RemoveCollider(Collider collider) => _spatialHash.Unregister(collider);

        public static void UpdateCollider(Collider collider)
        {
            _spatialHash.Unregister(collider);
            _spatialHash.Register(collider);
        }

        public static HashSet<Collider> CollisionBroadphase(RectangleF rect) =>
            _spatialHash.Aabb(rect, null);

        public static HashSet<Collider> CollisionBroadphaseExcludingSelf(Collider collider)
        {
            var bounds = collider.Bounds;
            return _spatialHash.Aabb(bounds, collider);
        }
    }
}
