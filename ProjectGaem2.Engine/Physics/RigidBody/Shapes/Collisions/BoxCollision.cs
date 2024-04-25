using System;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.Shapes;
using ProjectGaem2.Engine.Utils.Extensions;

namespace ProjectGaem2.Engine.Physics.RigidBody.Shapes.Collisions
{
    public static partial class Collision
    {
        public static bool Box2DToBox2D(Box2D first, Box2D second)
        {
            var d0 = second.Max.X <= first.Min.X;
            var d1 = first.Max.X <= second.Min.X;
            var d2 = second.Max.Y <= first.Min.Y;
            var d3 = first.Max.Y <= second.Min.Y;

            return !(d0 | d1 | d2 | d3);
        }

        public static bool Box2DToCapsule2D(
            Box2D box,
            Transform boxT,
            Capsule2D capsule,
            Transform capsuleT
        )
        {
            GJK.Compute(
                capsule,
                capsuleT,
                box,
                boxT,
                true,
                out GJKOutput output,
                out SimplexCache cache
            );
            return output.Distance < 10.0f * float.Epsilon;
        }

        public static bool Box2DToBox2DManifold(Box2D first, Box2D second, out Manifold manifold)
        {
            manifold = new Manifold();

            var firstCenter = (first.Min + first.Max) * 0.5f;
            var secondCenter = (second.Min + second.Max) * 0.5f;
            var dVector = secondCenter - firstCenter;
            var eFirst = Vector2Ext.Abs((first.Max - first.Min) * 0.5f);
            var eSecond = Vector2Ext.Abs((second.Max - second.Min) * 0.5f);

            var depthX = eFirst.X + eSecond.X - MathF.Abs(dVector.X);
            var depthY = eFirst.Y + eSecond.Y - MathF.Abs(dVector.Y);

            if (depthX <= 0 || depthY <= 0)
            {
                return false;
            }

            Vector2 normal;
            float depth;
            Vector2 contactPoint;

            if (depthX < depthY)
            {
                depth = depthX;
                if (dVector.X < 0)
                {
                    normal = new Vector2(-1.0f, 0);
                    contactPoint = firstCenter - new Vector2(eFirst.X, 0);
                }
                else
                {
                    normal = new Vector2(1.0f, 0);
                    contactPoint = firstCenter + new Vector2(eFirst.X, 0);
                }
            }
            else
            {
                depth = depthY;
                if (dVector.Y < 0)
                {
                    normal = new Vector2(0, -1.0f);
                    contactPoint = firstCenter - new Vector2(0, eFirst.Y);
                }
                else
                {
                    normal = new Vector2(0, 1.0f);
                    contactPoint = firstCenter + new Vector2(0, eFirst.Y);
                }
            }

            manifold.Count = 1;
            manifold.ContactPoints[0] = contactPoint;
            manifold.Depths[0] = depth;
            manifold.Normal = normal;

            return true;
        }
    }
}
