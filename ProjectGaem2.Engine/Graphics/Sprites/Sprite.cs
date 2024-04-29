using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGaem2.Engine.Graphics.Sprites
{
    public class Sprite(Texture2D texture, Rectangle sourceRectangle, Vector2 origin)
    {
        public Texture2D Texture { get; set; } = texture;
        public Rectangle SourceRectangle { get; } = sourceRectangle;
        public Vector2 Origin { get; set; } = origin;

        public Sprite(Texture2D texture, Rectangle sourceRectangle)
            : this(texture, sourceRectangle, sourceRectangle.Center.ToVector2()) { }

        public Sprite(Texture2D texture)
            : this(texture, new Rectangle(0, 0, texture.Width, texture.Height)) { }
    }
}
