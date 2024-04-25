using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Physics.RigidBody.Shapes
{
    public class Capsule2D : Shape
    {
        public Vector2 Start;
        public Vector2 End;
        public float Radius;

        public Capsule2D() { }

        public Capsule2D(Vector2 start, Vector2 end, float radius)
        {
            Start = start;
            End = end;
            Radius = radius;
        }
    }
}
