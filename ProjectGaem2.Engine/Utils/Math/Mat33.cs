using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Utils.Math
{
    public struct Mat33(Vector3 c1, Vector3 c2, Vector3 c3)
    {
        public Vector3 Ex = c1,
            Ey = c2,
            Ez = c3;

        public void SetZero()
        {
            Ex = Vector3.Zero;
            Ey = Vector3.Zero;
            Ez = Vector3.Zero;
        }

        public Vector3 Solve33(Vector3 b)
        {
            float det = Vector3.Dot(Ex, Vector3.Cross(Ey, Ez));
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }

            return new Vector3(
                det * Vector3.Dot(b, Vector3.Cross(Ey, Ez)),
                det * Vector3.Dot(Ex, Vector3.Cross(b, Ez)),
                det * Vector3.Dot(Ex, Vector3.Cross(Ey, b))
            );
        }

        public Vector2 Solve22(Vector2 b)
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

        public void GetInverse22(ref Mat33 M)
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

            M.Ex.X = det * d;
            M.Ey.X = -det * b;
            M.Ex.Z = 0.0f;
            M.Ex.Y = -det * c;
            M.Ey.Y = det * a;
            M.Ey.Z = 0.0f;
            M.Ez.X = 0.0f;
            M.Ez.Y = 0.0f;
            M.Ez.Z = 0.0f;
        }

        public void GetSymInverse33(ref Mat33 M)
        {
            float det = Vector3.Dot(Ex, Vector3.Cross(Ey, Ez));
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }

            float a11 = Ex.X,
                a12 = Ey.X,
                a13 = Ez.X;
            float a22 = Ey.Y,
                a23 = Ez.Y;
            float a33 = Ez.Z;

            M.Ex.X = det * (a22 * a33 - a23 * a23);
            M.Ex.Y = det * (a13 * a23 - a12 * a33);
            M.Ex.Z = det * (a12 * a23 - a13 * a22);

            M.Ey.X = M.Ex.Y;
            M.Ey.Y = det * (a11 * a33 - a13 * a13);
            M.Ey.Z = det * (a13 * a12 - a11 * a23);

            M.Ez.X = M.Ex.Z;
            M.Ez.Y = M.Ey.Z;
            M.Ez.Z = det * (a11 * a22 - a12 * a12);
        }
    }
}
