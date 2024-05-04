using System;
using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Physics
{
    public struct Rotation
    {
        /// Sine and cosine
        public float Sin,
            Cos;

        public Rotation(float angle)
        {
            Sin = (float)Math.Sin(angle);
            Cos = (float)Math.Cos(angle);
        }

        public Rotation(float sin, float cos)
        {
            Sin = sin;
            Cos = cos;
        }

        public static Rotation Identity => new(0, 1);

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
            Sin = 0;
            Cos = 1;
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
    }

    public struct PhysicsInternalTransform
    {
        public Vector2 Position;
        public Rotation Rotation;
        public float Scale;

        public static PhysicsInternalTransform Identity => new(Vector2.Zero, Rotation.Identity, 1);

        public PhysicsInternalTransform() { }

        public PhysicsInternalTransform(Vector2 position, Rotation rot, float scale)
        {
            Position = position;
            Rotation = rot;
            Scale = scale;
        }

        public void SetIdentity()
        {
            Position = Vector2.Zero;
            Rotation.SetIdentity();
            Scale = 1;
        }
    }
}
