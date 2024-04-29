using System;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.ECS.Entities;

namespace ProjectGaem2.Engine.ECS.Components
{
    public class Transform
    {
        public Entity Entity { get; set; }
        public Vector2 Position { get; set; }
        public Rotation Rotation { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;

        public static Transform Identity => new(Vector2.Zero, Rotation.Identity);

        public Transform() { }

        public Transform(Vector2 position, Rotation rot)
        {
            Position = position;
            Rotation = rot;
        }

        public void SetIdentity()
        {
            Position = Vector2.Zero;
            Rotation.SetIdentity();
        }
    }

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
}
