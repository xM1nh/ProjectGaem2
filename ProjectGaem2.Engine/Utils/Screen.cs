using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Utils
{
    public static class Screen
    {
        static GraphicsDeviceManager _graphics;

        internal static void Initialize(GraphicsDeviceManager graphics) => _graphics = graphics;

        public static int Width
        {
            get => _graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
            set => _graphics.GraphicsDevice.PresentationParameters.BackBufferWidth = value;
        }

        public static int Height
        {
            get => _graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
            set => _graphics.GraphicsDevice.PresentationParameters.BackBufferHeight = value;
        }

        public static Vector2 Center => new(Width / 2, Height / 2);
    }
}
