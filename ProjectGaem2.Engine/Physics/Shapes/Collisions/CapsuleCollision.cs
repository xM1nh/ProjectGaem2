using System;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Utils.Extensions;

namespace ProjectGaem2.Engine.Physics.Shapes.Collisions
{
    public static partial class Collision
    {
        public static bool Capsule2DToCapsule2D(
            Capsule2D first,
            PhysicsInternalTransform firstT,
            Capsule2D second,
            PhysicsInternalTransform secondT
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
            Capsule2D cap,
            PhysicsInternalTransform capT,
            Polygon poly,
            PhysicsInternalTransform polyT
        )
        {
            GJK.Compute(cap, capT, poly, polyT, true, out GJKOutput output, out SimplexCache cache);
            return output.Distance < 10f * Settings.Epsilon;
        }

        public static bool Capsule2DToCapsule2DManifold(
            Capsule2D first,
            PhysicsInternalTransform firstT,
            Capsule2D second,
            PhysicsInternalTransform secondT,
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
            Capsule2D cap,
            PhysicsInternalTransform capT,
            Polygon poly,
            PhysicsInternalTransform polyT,
            out Manifold manifold
        )
        {
            throw new NotImplementedException();
        }

        //static void AntinormalFace(
        //    Capsule2D cap,
        //    Polygon poly,
        //    PhysicsInternalTransform polyT,
        //    out int index,
        //    out Vector2 normal
        //)
        //{
        //    index = -1;
        //    normal = Vector2.Zero;
        //    float sep = float.MinValue;

        //    for (int i = 0; i < poly.Vertices.Count; i++)
        //    {
        //        var h = HalfSpace.At(poly, i).MulT(polyT);
        //        var n0 = Vector2.Negate(h.Normal);
        //        var s = CapsuleSupport(cap, n0);
        //        var d = h.DistanceFromPoint(s);

        //        if (d > sep)
        //        {
        //            sep = d;
        //            index = i;
        //            normal = n0;
        //        }
        //    }
        //}

        //static Vector2 CapsuleSupport(Capsule2D cap, Vector2 dir)
        //{
        //    var da = Vector2.Dot(cap.Start, dir);
        //    var db = Vector2.Dot(cap.End, dir);

        //    if (da > db)
        //    {
        //        return cap.Start + dir * cap.Radius;
        //    }

        //    return cap.End - dir * cap.Radius;
        //}
    }
}
