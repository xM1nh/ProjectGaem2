using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.Physics.Shapes
{
    public class Box2D : Shape
    {
        public Vector2 Min;
        public Vector2 Max;

        public Box2D()
            : base()
        {
            Min = Vector2.Zero;
            Max = Vector2.One;
        }

        public Box2D(float width, float height)
            : base()
        {
            Min = Vector2.Zero;
            Max = new Vector2(width, height);
        }

        public Box2D(Vector2 min, Vector2 max)
            : base()
        {
            Min = min;
            Max = max;
        }

        public override void CalculateBounds()
        {
            Bounds = new RectangleF(Min.X, Min.Y, Max.X - Min.X, Max.Y - Min.Y);
        }

        public void SetWidth(float width) => Max.X = width;

        public void SetHeight(float height) => Max.Y = height;

        public override void SetTransform(Vector2 position, float rotation = 0)
        {
            Min = position;
            Max = position;
        }
    }
}
