using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Utils.Extensions
{
    public static class RectangleExt
    {
        public static void Union(ref Rectangle rect, ref Point point, out Rectangle result)
        {
            var pointRect = new Rectangle(point.X, point.Y, 0, 0);
            Rectangle.Union(ref rect, ref pointRect, out result);
        }
    }
}
