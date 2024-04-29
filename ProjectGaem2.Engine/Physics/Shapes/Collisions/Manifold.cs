using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Physics.Shapes.Collisions
{
    public struct Manifold
    {
        public int Count;
        public float[] Depths;
        public Vector2[] ContactPoints;
        public Vector2 Normal;
        public Vector2 MinimumTranslationVector;

        public Manifold()
        {
            Depths = new float[2];
            ContactPoints = new Vector2[2];
            MinimumTranslationVector = -Depths[0] * Normal;
        }

        public void Invert() => Vector2.Negate(ref Normal, out Normal);
    }
}
