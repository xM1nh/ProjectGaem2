﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Physics.Shapes
{
    public class Polygon : Shape
    {
        List<Vector2> _vertices;
        List<Vector2> _normals;

        public List<Vector2> Vertices
        {
            get => _vertices;
            set => SetVertices(value);
        }
        public List<Vector2> Normals
        {
            get => _normals;
        }

        public Polygon()
            : base() { }

        public Polygon(List<Vector2> vertices)
            : base()
        {
            SetVertices(vertices);
        }

        public override void CalculateBounds()
        {
            throw new System.NotImplementedException();
        }

        public void SetVertices(List<Vector2> vertices)
        {
            _vertices = vertices;
            if (_normals is null)
            {
                _normals = [];
            }
            else
            {
                _normals.Clear();
            }

            for (var i = 0; i < _vertices.Count; i++)
            {
                var next = i + 1 < _vertices.Count ? i + 1 : 0;
                var edge = _vertices[next] - _vertices[i];

                var temp = new Vector2(edge.Y, -edge.X);
                temp.Normalize();
                _normals.Add(temp);
            }
        }

        public override void SetTransform(Vector2 position, float rotation = 0)
        {
            Transform.Position = position;

            if (rotation != 0)
            {
                Transform.Rotation.Set(rotation);
            }
        }
    }
}
