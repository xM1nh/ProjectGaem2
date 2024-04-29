using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine.Graphics.Sprites;

namespace ProjectGaem2.Engine.ECS.Components.Renderables
{
    public class SpriteRenderer : RenderableComponent
    {
        public Sprite Sprite { get; set; }
        public Color Color { get; set; } = Color.White;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public float Layer { get; set; } = 1;

        public SpriteRenderer() { }

        public SpriteRenderer(Texture2D texture)
        {
            Sprite = new(texture);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Sprite.Texture,
                Entity.Transform.Position,
                Sprite.SourceRectangle,
                Color,
                Entity.Transform.Rotation.GetAngle(),
                Sprite.Origin,
                Entity.Transform.Scale,
                Effects,
                Layer
            );
        }
    }
}
