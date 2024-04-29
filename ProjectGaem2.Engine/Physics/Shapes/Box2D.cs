using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.Physics.Shapes
{
    public class Box2D : Shape
    {
        public Vector2 Min;
        public Vector2 Max;

        public Box2D()
        {
            Min = Vector2.Zero;
            Max = Vector2.One;
        }

        public Box2D(float width, float height)
        {
            Min = Vector2.Zero;
            Max = new Vector2(width, height);
        }

        public Box2D(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public override void CalculateBounds()
        {
            Bounds = new RectangleF(Min.X, Min.Y, Max.X - Min.X, Max.Y - Min.Y);
        }
    }
}
