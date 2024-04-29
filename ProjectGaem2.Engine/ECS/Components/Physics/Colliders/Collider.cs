using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.ECS.Entities;
using ProjectGaem2.Engine.Physics.Shapes;
using ProjectGaem2.Engine.Physics.Shapes.Collisions;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.ECS.Components.Physics.Colliders
{
    public class Collider : Component
    {
        public Shape Shape { get; protected set; }
        public RectangleF Bounds => Shape.Bounds;
        public RectangleF RegisteredBounds;
        public Vector2 Offset { get; set; } = Vector2.Zero;

        private Transform _transform;

        public Collider()
        {
            var position = Entity.Transform.Position + Offset;
            _transform = new Transform(position, Entity.Transform.Rotation);
        }

        public bool Collides(Collider other, out Manifold manifold) =>
            Collision.Collides(Shape, _transform, other.Shape, other._transform, out manifold);

        public bool Collides(Collider other, Vector2 motion, out Manifold manifold)
        {
            var newTransform = new Transform(_transform.Position + motion, _transform.Rotation);

            return Collision.Collides(
                Shape,
                newTransform,
                other.Shape,
                other._transform,
                out manifold
            );
        }

        public bool Overlaps(Collider other) =>
            Collision.Overlaps(Shape, _transform, other.Shape, other._transform);
    }
}
