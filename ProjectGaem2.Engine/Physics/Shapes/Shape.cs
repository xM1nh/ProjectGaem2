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

        public Shape()
        {
            Transform = PhysicsInternalTransform.Identity;
        }

        public virtual void SetTransform(Vector2 position, float rotation = 0) { }

        public void ResetTransform() => Transform.SetIdentity();

        public bool Collides(Shape other, out Manifold manifold) =>
            Collision.Collides(this, other, out manifold);

        public bool Overlaps(Shape other) => Collision.Overlaps(this, other);
    }
}
