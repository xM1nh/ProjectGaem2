using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGaem2.Engine.ECS.Components.Renderables
{
    public class RenderableComponent : Component, IRenderable
    {
        public bool Visible { get; set; }

        public virtual Rectangle Bounds { get; set; }

        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
