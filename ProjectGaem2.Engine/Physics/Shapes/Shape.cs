using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.Physics.Shapes
{
    public abstract class Shape
    {
        public RectangleF Bounds;

        public virtual void CalculateBounds() { }
    }
}
