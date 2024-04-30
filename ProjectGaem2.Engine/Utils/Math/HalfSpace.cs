using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Physics.Shapes;
using ProjectGaem2.Engine.Physics.Shapes.Collisions;

namespace ProjectGaem2.Engine.Utils.Math
{
    public struct HalfSpace(Vector2 normal, float distance)
    {
        public Vector2 Normal = normal;
        public float Distance = distance;

        public static HalfSpace At(Polygon poly, int i)
        {
            var halfPlane = new HalfSpace
            {
                Normal = poly.Normals[i],
                Distance = Vector2.Dot(poly.Normals[i], poly.Vertices[i])
            };
            return halfPlane;
        }

        public Vector2 Origin()
        {
            return Normal * Distance;
        }

        public float DistanceFromPoint(Vector2 p)
        {
            return Vector2.Dot(Normal, p) - Distance;
        }

        public Vector2 Project(Vector2 p)
        {
            return p - Normal * DistanceFromPoint(p);
        }

        public HalfSpace MulT(PhysicsInternalTransform t)
        {
            var result = new HalfSpace();
            result.Normal = GJKHelper.Mul(t.Rotation, Normal);
            result.Distance = Vector2.Dot(GJKHelper.Mul(t, Origin()), result.Normal);

            return result;
        }
    }
}
