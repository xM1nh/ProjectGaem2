using System;
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
