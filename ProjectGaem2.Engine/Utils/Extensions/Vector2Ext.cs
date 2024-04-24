using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Utils.Extensions
{
    public class Vector2Ext
    {
        public static float Cross(Vector2 u, Vector2 v)
        {
            return Cross(ref u, ref v);
        }

        public static float Cross(ref Vector2 u, ref Vector2 v)
        {
            return u.Y * v.X - u.X * v.Y;
        }

        public static Vector2 Skew(Vector2 u)
        {
            return new Vector2(-u.Y, u.X);
        }
    }
}
