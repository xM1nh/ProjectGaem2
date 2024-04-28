using System;
using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Physics
{
    public struct Rot
    {
        /// Sine and cosine
        public float Sin,
            Cos;

        public Rot(float angle)
        {
            Sin = (float)Math.Sin(angle);
            Cos = (float)Math.Cos(angle);
        }

        public Rot(float sin, float cos)
        {
            Sin = sin;
            Cos = cos;
        }

        public void Set(float angle)
        {
            //FPE: Optimization
            if (angle == 0)
            {
                Sin = 0;
                Cos = 1;
            }
            else
            {
                // TODO_ERIN optimize
                Sin = (float)Math.Sin(angle);
                Cos = (float)Math.Cos(angle);
            }
        }

        public void SetIdentity()
        {
            Sin = 0.0f;
            Cos = 1.0f;
        }

        public float GetAngle()
        {
            return (float)Math.Atan2(Sin, Cos);
        }

        public Vector2 GetXAxis()
        {
            return new Vector2(Cos, Sin);
        }

        public Vector2 GetYAxis()
        {
            return new Vector2(-Sin, Cos);
        }

        public Vector2 MulV(Vector2 v)
        {
            return new Vector2(Cos * v.X + Sin * v.Y, -Sin * v.X + Cos * v.Y);
        }

        public Rot Mul(Rot other)
        {
            var result = new Rot
            {
                Cos = Cos * other.Cos + Sin * other.Sin,
                Sin = Cos * other.Sin - Sin * other.Cos
            };
            return result;
        }
    }

    public struct Transform
    {
        public Vector2 Position;
        public Rot Rotation;
        public Vector2 Scale;

        public Transform(in Vector2 position, in Rot rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public Transform(in Vector2 position, float angle)
        {
            Position = position;
            Rotation = new Rot(angle);
        }

        public void SetIdentity()
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation.SetIdentity();
        }

        public static Transform Identity()
        {
            var t = new Transform();
            t.SetIdentity();
            return t;
        }

        public void Set(Vector2 position, float angle)
        {
            Position = position;
            Rotation.Set(angle);
        }

        public Transform Mul(Transform other)
        {
            var result = new Transform
            {
                Rotation = Rotation.Mul(other.Rotation),
                Position = Rotation.MulV((other.Position - Position))
            };

            return result;
        }

        public Vector2 MulV(Vector2 v)
        {
            return MulV(ref v);
        }

        public Vector2 MulV(ref Vector2 v)
        {
            float x = (Rotation.Cos * v.X - Rotation.Sin * v.Y) + Position.X;
            float y = (Rotation.Sin * v.X + Rotation.Cos * v.Y) + Position.Y;

            return new Vector2(x, y);
        }
    }
}
