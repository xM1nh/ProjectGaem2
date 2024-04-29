using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.Physics.Shapes
{
    public abstract class Shape
    {
        public RectangleF Bounds;

        public abstract void CalculateBounds();
    }
}
