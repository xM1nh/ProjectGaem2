namespace ProjectGaem2.Engine.Graphics.Sprites
{
    public class SpriteAnimation
    {
        public readonly Sprite[] Sprites;
        public readonly float[] FrameRates;

        public SpriteAnimation(Sprite[] sprites, float[] frameRates)
        {
            Sprites = sprites;
            FrameRates = frameRates;
        }

        public SpriteAnimation(Sprite[] sprites, float frameRates)
        {
            Sprites = sprites;
            FrameRates = new float[Sprites.Length];
            for (int i = 0; i < Sprites.Length; i++)
            {
                FrameRates[i] = frameRates;
            }
        }
    }
}
