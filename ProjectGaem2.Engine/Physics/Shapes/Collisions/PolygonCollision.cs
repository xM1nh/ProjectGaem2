using System;

namespace ProjectGaem2.Engine.Physics.Shapes.Collisions
{
    public static partial class Collision
    {
        public static bool PolygonToPolygon(Polygon first, Polygon second)
        {
            GJK.Compute(
                first,
                first.Transform,
                second,
                second.Transform,
                true,
                out GJKOutput output,
                out SimplexCache cache
            );
            return output.Distance < 10f * float.Epsilon;
        }

        public static bool PolygonToPolygonManifold(
            Polygon first,
            Polygon second,
            out Manifold manifold
        )
        {
            throw new NotImplementedException();
        }
    }
}
