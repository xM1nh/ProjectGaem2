using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.Utils.Extensions
{
    public class Vector2Ext
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Skew(Vector2 u)
        {
            return new Vector2(-u.Y, u.X);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Abs(Vector2 u)
        {
            u.X = MathF.Abs(u.X);
            u.Y = MathF.Abs(u.Y);
            return u;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Abs(ref Vector2 u, out Vector2 result)
        {
            result.X = MathF.Abs(u.X);
            result.Y = MathF.Abs(u.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Transform(Vector2 position, Matrix2 matrix)
        {
            return new Vector2(
                (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M31,
                (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M32
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(ref Vector2 position, ref Matrix2 matrix, out Vector2 result)
        {
            var x = (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M31;
            var y = (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M32;
            result.X = x;
            result.Y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Perpendicular(in Vector2 first, in Vector2 second)
        {
            return new Vector2(-1f * (second.Y - first.Y), second.X - first.X);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Perpendicular(in Vector2 original)
        {
            return new Vector2(-original.Y, original.X);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cross(in Vector2 a, in Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Cross(in float a, in Vector2 v)
        {
            return new(-a * v.Y, a * v.X);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Cross(in Vector2 v, in float a)
        {
            return new(a * v.Y, -a * v.X);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equal(in Vector2 u, in Vector2 v)
        {
            return Vector2.DistanceSquared(u, v) < 0.0005f * 0.0005f;
        }
    }
}
