using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.Shapes;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.Utils.Extensions
{
    public static class MathHelperExt
    {
        public static Vector2 Mul(ref Mat22 A, Vector2 v)
        {
            return Mul(ref A, ref v);
        }

        public static Vector2 Mul(ref Mat22 A, ref Vector2 v)
        {
            return new Vector2(A.Ex.X * v.X + A.Ey.X * v.Y, A.Ex.Y * v.X + A.Ey.Y * v.Y);
        }

        public static Vector2 Mul(ref Transform T, Vector2 v)
        {
            return Mul(ref T, ref v);
        }

        public static Vector2 Mul(ref Transform T, ref Vector2 v)
        {
            float x = (T.Rotation.Cos * v.X - T.Rotation.Sine * v.Y) + T.Position.X;
            float y = (T.Rotation.Sine * v.X + T.Rotation.Cos * v.Y) + T.Position.Y;

            return new Vector2(x, y);
        }

        public static Vector2 MulT(ref Mat22 A, Vector2 v)
        {
            return MulT(ref A, ref v);
        }

        public static Vector2 MulT(ref Mat22 A, ref Vector2 v)
        {
            return new Vector2(v.X * A.Ex.X + v.Y * A.Ex.Y, v.X * A.Ey.X + v.Y * A.Ey.Y);
        }

        /// Inverse rotate a vector
        public static Vector2 MulT(Rot q, Vector2 v)
        {
            return new Vector2(q.Cos * v.X + q.Sine * v.Y, -q.Sine * v.X + q.Cos * v.Y);
        }
    }
}
