using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.Physics.Shapes
{
    public class Capsule2D : Shape
    {
        public Vector2 Start;
        public Vector2 End;
        public float Radius;

        public Capsule2D(Vector2 end)
        {
            Start = Vector2.Zero;
            End = end;
            Radius = 1;
        }

        public Capsule2D(Vector2 direction, float radius)
        {
            Start = Vector2.Zero;
            End = direction;
            Radius = radius;
        }

        public Capsule2D(Vector2 start, Vector2 end, float radius)
        {
            Start = start;
            End = end;
            Radius = radius;
        }

        public override void CalculateBounds()
        {
            Bounds = new RectangleF(
                Start.X - Radius,
                Start.Y - Radius,
                2.0f * Radius,
                2.0f * Radius + (Start - End).Length()
            );
        }
    }
}
