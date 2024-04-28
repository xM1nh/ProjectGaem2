using System;
using ProjectGaem2.Engine.ECS.Components;

namespace ProjectGaem2.Engine.Physics.Shapes.Collisions
{
    public static partial class Collision
    {
        public static bool PolygonToPolygon(
            Polygon first,
            Transform firstT,
            Polygon second,
            Transform secondT
        )
        {
            GJK.Compute(
                first,
                firstT,
                second,
                secondT,
                true,
                out GJKOutput output,
                out SimplexCache cache
            );
            return output.Distance < 10f * float.Epsilon;
        }

        public static bool PolygonToPolygonManifold(
            Polygon first,
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
