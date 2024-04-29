using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.Physics.Shapes
{
    public class Circle : Shape
    {
        public Vector2 Center;
        public float Radius;

        public Circle()
        {
            Center = Vector2.Zero;
            Radius = 1;
        }

        public Circle(float radius)
        {
            Radius = radius;
            Center = Vector2.Zero;
        }

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public override void CalculateBounds()
        {
            Bounds = new RectangleF(
                Center.X - Radius,
                Center.Y - Radius,
                2.0f * Radius,
                2.0f * Radius
            );
        }
    }
}
