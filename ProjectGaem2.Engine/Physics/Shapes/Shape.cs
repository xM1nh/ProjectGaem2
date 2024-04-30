using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.Shapes.Collisions;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.Physics.Shapes
{
    public abstract class Shape
    {
        public RectangleF Bounds;
        public PhysicsInternalTransform Transform;
        public abstract void CalculateBounds();

        public void SetTransform(Vector2 position, float rotation = 0)
        {
            Transform.Position = position;

            if (rotation != 0)
            {
                Transform.Rotation.Set(rotation);
            }
        }

        public void ResetTransform() => Transform.SetIdentity();

        public bool Collides(Shape other, out Manifold manifold) =>
            Collision.Collides(this, Transform, other, other.Transform, out manifold);

        public bool Overlaps(Shape other) =>
            Collision.Overlaps(this, Transform, other, other.Transform);
    }
}
