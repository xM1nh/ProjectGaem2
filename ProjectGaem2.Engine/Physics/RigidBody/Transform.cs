using System;
using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Physics.Shapes
{
    public struct Rot(float angle)
    {
        /// Sine and cosine
        public float Sine = (float)Math.Sin(angle),
            Cos = (float)Math.Cos(angle);

        public void Set(float angle)
        {
            //FPE: Optimization
            if (angle == 0)
            {
                Sine = 0;
                Cos = 1;
            }
            else
            {
                // TODO_ERIN optimize
                Sine = (float)Math.Sin(angle);
                Cos = (float)Math.Cos(angle);
            }
        }

        public void SetIdentity()
        {
            Sine = 0.0f;
            Cos = 1.0f;
        }

        public float GetAngle()
        {
            return (float)Math.Atan2(Sine, Cos);
        }

        public Vector2 GetXAxis()
        {
            return new Vector2(Cos, Sine);
        }

        public Vector2 GetYAxis()
        {
            return new Vector2(-Sine, Cos);
        }
    }

    public struct Transform
    {
        public Vector2 Position;
        public Rot Rotation;

        public Transform(ref Vector2 position, ref Rot rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public void SetIdentity()
        {
            Position = Vector2.Zero;
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
    }
}
