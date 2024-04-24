using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Utils.Math
{
    public struct Mat22
    {
        public Vector2 Ex,
            Ey;

        public Mat22(Vector2 c1, Vector2 c2)
        {
            Ex = c1;
            Ey = c2;
        }

        public Mat22(float a11, float a12, float a21, float a22)
        {
            Ex = new Vector2(a11, a21);
            Ey = new Vector2(a12, a22);
        }

        public Mat22 Inverse
        {
            get
            {
                float a = Ex.X,
                    b = Ey.X,
                    c = Ex.Y,
                    d = Ey.Y;
                float det = a * d - b * c;
                if (det != 0.0f)
                {
                    det = 1.0f / det;
                }

                Mat22 result = new Mat22();
                result.Ex.X = det * d;
                result.Ex.Y = -det * c;

                result.Ey.X = -det * b;
                result.Ey.Y = det * a;

                return result;
            }
        }

        public void Set(Vector2 c1, Vector2 c2)
        {
            Ex = c1;
            Ey = c2;
        }

        public void SetIdentity()
        {
            Ex.X = 1.0f;
            Ey.X = 0.0f;
            Ex.Y = 0.0f;
            Ey.Y = 1.0f;
        }

        public void SetZero()
        {
            Ex.X = 0.0f;
            Ey.X = 0.0f;
            Ex.Y = 0.0f;
            Ey.Y = 0.0f;
        }

        public Vector2 Solve(Vector2 b)
        {
            float a11 = Ex.X,
                a12 = Ey.X,
                a21 = Ex.Y,
                a22 = Ey.Y;
            float det = a11 * a22 - a12 * a21;
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }

            return new Vector2(det * (a22 * b.X - a12 * b.Y), det * (a11 * b.Y - a21 * b.X));
        }

        public static void Add(ref Mat22 A, ref Mat22 B, out Mat22 R)
        {
            R.Ex = A.Ex + B.Ex;
            R.Ey = A.Ey + B.Ey;
        }
    }
}
