﻿using System.Diagnostics.Contracts;
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

        /// Multiply a matrix transpose times a vector. If a rotation matrix is provided,
        /// then this transforms the vector from one frame to another (inverse transform).
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector2 MulT(in Mat22 m, in Vector2 v)
        {
            return new Vector2(Vector2.Dot(v, m.Ex), Vector2.Dot(v, m.Ey));
        }

        // A * B
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Mat22 Mul(in Mat22 a, in Mat22 b)
        {
            return new Mat22(Mul(a, b.Ex), Mul(a, b.Ey));
        }

        // A^T * B
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Mat22 MulT(in Mat22 a, in Mat22 b)
        {
            return new Mat22(
                new Vector2(Vector2.Dot(a.Ex, b.Ex), Vector2.Dot(a.Ey, b.Ex)),
                new Vector2(Vector2.Dot(a.Ex, b.Ey), Vector2.Dot(a.Ey, b.Ey))
            );
        }

        /// Multiply a matrix times a vector.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector3 Mul(in Mat33 m, in Vector3 v)
        {
            return v.X * m.Ex + v.Y * m.Ey + v.Z * m.Ez;
        }

        /// Multiply a matrix times a vector.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector2 Mul22(in Mat33 m, in Vector2 v)
        {
            return new Vector2(m.Ex.X * v.X + m.Ey.X * v.Y, m.Ex.Y * v.X + m.Ey.Y * v.Y);
        }

        /// Multiply two rotations: q * r
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Rotation Mul(in Rotation q, in Rotation r)
        {
            // [qc -qs] * [rc -rs] = [qc*rc-qs*rs -qc*rs-qs*rc]
            // [qs  qc]   [rs  rc]   [qs*rc+qc*rs -qs*rs+qc*rc]
            // s = qs * rc + qc * rs
            // c = qc * rc - qs * rs
            return new Rotation(q.Sin * r.Cos + q.Cos * r.Sin, q.Cos * r.Cos - q.Sin * r.Sin);
        }

        /// Transpose multiply two rotations: qT * r
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Rotation MulT(in Rotation q, in Rotation r)
        {
            // [ qc qs] * [rc -rs] = [qc*rc+qs*rs -qc*rs+qs*rc]
            // [-qs qc]   [rs  rc]   [-qs*rc+qc*rs qs*rs+qc*rc]
            // s = qc * rs - qs * rc
            // c = qc * rc + qs * rs
            return new Rotation(q.Cos * r.Sin - q.Sin * r.Cos, q.Cos * r.Cos + q.Sin * r.Sin);
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
            var x = T.Rotation.Cos * v.X - T.Rotation.Sin * v.Y + T.Position.X;
            var y = T.Rotation.Sin * v.X + T.Rotation.Cos * v.Y + T.Position.Y;
            return new Vector2(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector2 MulT(in PhysicsInternalTransform T, in Vector2 v)
        {
            var px = v.X - T.Position.X;
            var py = v.Y - T.Position.Y;
            return new Vector2(
                T.Rotation.Cos * px + T.Rotation.Sin * py,
                -T.Rotation.Sin * px + T.Rotation.Cos * py
            );
        }

        // v2 = A.Rotation.Rotation(B.Rotation.Rotation(v1) + B.p) + A.p
        //    = (A.q * B.q).Rotation(v1) + A.Rotation.Rotation(B.p) + A.p
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static PhysicsInternalTransform Mul(
            in PhysicsInternalTransform A,
            in PhysicsInternalTransform B
        )
        {
            return new PhysicsInternalTransform(
                Mul(A.Rotation, B.Position) + A.Position,
                Mul(A.Rotation, B.Rotation)
            );
        }

        // v2 = A.q' * (B.q * v1 + B.p - A.p)
        //    = A.q' * B.q * v1 + A.q' * (B.p - A.p)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static PhysicsInternalTransform MulT(
            in PhysicsInternalTransform A,
            in PhysicsInternalTransform B
        )
        {
            return new PhysicsInternalTransform(
                MulT(A.Rotation, B.Position - A.Position),
                MulT(A.Rotation, B.Rotation)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float Clamp(float a, float low, float high)
        {
            return a < low
                ? low
                : a > high
                    ? high
                    : a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Swap<T>(ref T a, ref T b)
        {
            var tmp = a;
            a = b;
            b = tmp;
        }

        /// "Next Largest Power of 2
        /// Given a binary integer value x, the next largest power of 2 can be computed by a SWAR algorithm
        /// that recursively "folds" the upper bits into the lower bits. This process yields a bit vector with
        /// the same most significant 1 as x, but all 1's below it. Adding 1 to that value yields the next
        /// largest power of 2. For a 32-bit value:"
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint NextPowerOfTwo(uint x)
        {
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            return x + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsPowerOfTwo(uint x)
        {
            var result = x > 0 && (x & (x - 1)) == 0;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        internal static int GetArraySize(int capacity)
        {
            var n = capacity - 1;

            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;
            return n < 0 ? 128 : n + 1;
        }
    }
}
