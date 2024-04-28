using System;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Utils.Extensions;

namespace ProjectGaem2.Engine.Physics.Shapes.Collisions
{
    public static partial class Collision
    {
        public static bool Capsule2DToCapsule2D(
            Capsule2D first,
            Transform firstT,
            Capsule2D second,
            Transform secondT
        )
        {
            GJK.Compute(
                first,
                firstT,
                second,
                secondT,
                false,
                out GJKOutput output,
                out SimplexCache cache
            );
            return output.Distance < first.Radius + second.Radius;
        }

        public static bool Capsule2DToPolygon(
            Capsule2D first,
            Transform firstT,
            Polygon second,
            Transform secondT
        )
        {
            //GJK.Compute(
            //    first,
            //    firstT,
            //    second,
            //    secondT,
            //    true,
            //    out GJKOutput output,
            //    out SimplexCache cache
            //);
            //return output.Distance < 10f * float.Epsilon;

            throw new NotImplementedException();
        }

        public static bool Capsule2DToCapsule2DManifold(
            Capsule2D first,
            Transform firstT,
            Capsule2D second,
            Transform secondT,
            out Manifold manifold
        )
        {
            manifold = new Manifold();
            var radiiSum = first.Radius + second.Radius;

            GJK.Compute(
                first,
                firstT,
                second,
                secondT,
                false,
                out GJKOutput output,
                out SimplexCache cache
            );

            if (output.Distance < radiiSum)
            {
                Vector2 normal;
                if (output.Distance == 0)
                {
                    normal = Vector2.Normalize(Vector2Ext.Skew(first.End - first.Start));
                }
                else
                {
                    normal = Vector2.Normalize(output.PointB - output.PointA);
                }

                manifold.Count = 1;
                manifold.Depths[0] = radiiSum - output.Distance;
                manifold.ContactPoints[0] = output.PointB - normal * second.Radius;
                manifold.Normal = normal;

                return true;
            }

            return false;
        }

        public static bool Capsule2DToPolygonManifold(
            Capsule2D first,
            Transform firstT,
            Polygon second,
            Transform secondT,
            out Manifold manifold
        )
        {
            throw new NotImplementedException();
        }
    }
}
