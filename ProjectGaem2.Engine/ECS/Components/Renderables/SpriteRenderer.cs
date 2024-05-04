using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine.Graphics.Sprites;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.ECS.Components.Renderables
{
    public class SpriteRenderer : RenderableComponent
    {
        public Sprite Sprite { get; set; }
        public Color Color { get; set; } = Color.White;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public float Layer { get; set; } = 1;

        public override RectangleF Bounds
        {
            get
            {
                if (_boundsDirty)
                {
                    if (Sprite is not null)
                        _bounds.CalculateBounds(
                            Entity.Position,
                            Vector2.Zero,
                            Entity.Scale,
                            Entity.Rotation,
                            Sprite.SourceRectangle.Width,
                            Sprite.SourceRectangle.Height
                        );
                    _boundsDirty = false;
                }

                return _bounds;
            }
        }

        public SpriteRenderer() { }

        public SpriteRenderer(Texture2D texture)
        {
            Sprite = new(texture);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Sprite.Texture,
                Entity.Position,
                Sprite.SourceRectangle,
                Color,
                Entity.Rotation,
                Sprite.Origin,
                Entity.Scale,
                Effects,
                Layer
            );
        }
    }
}
