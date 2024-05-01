using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.Physics.Shapes.Collisions
{
    internal static class GJKHelper
    {
        /// Perform the cross product on two vectors. In 2D this produces a scalar.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float Cross(in Vector2 a, in Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        /// Perform the cross product on a vector and a scalar. In 2D this produces
        /// a vector.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector2 Cross(in Vector2 a, float s)
        {
            return new Vector2(s * a.Y, -s * a.X);
        }

        /// Perform the cross product on a scalar and a vector. In 2D this produces
        /// a vector.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector2 Cross(float s, in Vector2 a)
        {
            return new Vector2(-s * a.Y, s * a.X);
        }

        /// Multiply a matrix times a vector. If a rotation matrix is provided,
        /// then this transforms the vector from one frame to another.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector2 Mul(in Mat22 m, in Vector2 v)
        {
            return new Vector2(m.Ex.X * v.X + m.Ey.X * v.Y, m.Ex.Y * v.X + m.Ey.Y * v.Y);
        }

        /// Rotationate a vector
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector2 Mul(in Rotation q, in Vector2 v)
        {
            return new Vector2(q.Cos * v.X - q.Sin * v.Y, q.Sin * v.X + q.Cos * v.Y);
        }

        /// Inverse rotate a vector
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector2 MulT(in Rotation q, in Vector2 v)
        {
            return new Vector2(q.Cos * v.X + q.Sin * v.Y, -q.Sin * v.X + q.Cos * v.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector2 Mul(in PhysicsInternalTransform T, in Vector2 v)
        {
            var x =
                T.Rotation.Cos * T.Scale.X * v.X - T.Rotation.Sin * T.Scale.Y * v.Y + T.Position.X;
            var y =
                T.Rotation.Sin * T.Scale.X * v.X + T.Rotation.Cos * T.Scale.Y * v.Y + T.Position.Y;
            return new Vector2(x, y);
        }
    }
}
