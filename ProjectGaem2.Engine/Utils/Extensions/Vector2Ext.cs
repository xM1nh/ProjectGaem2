using System;
using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Utils.Extensions
{
    public class Vector2Ext
    {
        public static Vector2 Skew(Vector2 u)
        {
            return new Vector2(-u.Y, u.X);
        }

        public static Vector2 Abs(Vector2 u)
        {
            u.X = MathF.Abs(u.X);
            u.Y = MathF.Abs(u.Y);
            return u;
        }

        public static void Abs(ref Vector2 u, out Vector2 result)
        {
            result.X = MathF.Abs(u.X);
            result.Y = MathF.Abs(u.Y);
        }
    }
}
