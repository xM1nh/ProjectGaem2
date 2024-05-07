using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Physics.Shapes.Collisions
{
    public struct RaycastManifold
    {
        public Vector2 Point;

        public Vector2 Normal;

        public float Lambda;

        public int Iterations;
    }
}
