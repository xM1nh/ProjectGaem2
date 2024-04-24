using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.RigidBody.Shapes;
using ProjectGaem2.Engine.Physics.RigidBody.Shapes.Collisions;
using ProjectGaem2.Engine.Physics.Shapes;

namespace ProjectGaem2.Engine.Physics.RigidBody
{
    public abstract class Body
    {
        public Shape Shape;
        public Transform Transform;
        public BoundingBox Bound;

        public virtual bool Overlaps(Body other) =>
            Collision.Overlaps(Shape, Transform, other.Shape, other.Transform);

        public virtual bool Collides(Body other, out Manifold manifold) =>
            Collision.Collides(Shape, Transform, other.Shape, other.Transform, out manifold);
    }
}
