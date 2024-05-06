using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.Physics.Shapes
{
    public class Box2D : Shape
    {
        public Vector2 Min;
        public Vector2 Max;

        public float Width
        {
            get => Max.X - Min.X;
            set => Max.X = value;
        }

        public float Height
        {
            get => Max.Y - Min.Y;
            set => Max.Y = value;
        }

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

        public override void SetTransform(Vector2 position, float rotation = 0)
        {
            var width = Width;
            var height = Height;

            Min = new Vector2(position.X - width / 2, position.Y - height / 2);
            Max = new Vector2(position.X + width / 2, position.Y + height / 2);
        }
    }
}
