using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Physics.RigidBody.Shapes
{
    public class Circle : Shape
    {
        public Vector2 Center;
        public float Radius;

        public Circle() { }

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }
    }
}
