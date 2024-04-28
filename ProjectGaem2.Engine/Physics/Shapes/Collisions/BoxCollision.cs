using System;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.ECS.Components;
using ProjectGaem2.Engine.Utils.Extensions;

namespace ProjectGaem2.Engine.Physics.Shapes.Collisions
{
    public static partial class Collision
    {
        public static bool Box2DToBox2D(Box2D first, Box2D second)
        {
            return second.Min.X < first.Max.X
                && first.Min.X < second.Max.X
                && second.Min.Y < first.Max.Y
                && first.Min.Y < second.Max.Y;
        }

        public static bool Box2DToCapsule2D(
            Box2D box,
            Transform boxT,
            Capsule2D capsule,
            Transform capsuleT
        )
        {
            //throw new NotImplementedException();
            GJK.Compute(
                box,
                boxT,
                capsule,
                capsuleT,
                true,
                out GJKOutput output,
                out SimplexCache cache
            );
            return output.Distance < 10.0f * Settings.Epsilon;
        }

        public static bool Box2DToPolygon(Box2D box, Transform boxT, Polygon poly, Transform polyT)
        {
            GJK.Compute(box, boxT, poly, polyT, true, out GJKOutput output, out SimplexCache cache);

            return output.Distance < 10.0f * Settings.Epsilon;
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

            //If depth = 0, just treat this as non-collision
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

        public static bool Box2DToCapsule2DManifold(
            Box2D box,
            Transform boxT,
            Capsule2D capsule,
            Transform capsuleT,
            out Manifold manifold
        )
        {
            throw new NotImplementedException();
        }

        public static bool Box2DToPolygonManifold(
            Box2D box,
            Transform boxT,
            Polygon poly,
            Transform polyT,
            out Manifold manifold
        )
        {
            throw new NotImplementedException();
        }
    }
}
