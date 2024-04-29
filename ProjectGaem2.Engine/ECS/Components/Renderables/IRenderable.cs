using Microsoft.Xna.Framework.Graphics;

namespace ProjectGaem2.Engine.ECS.Components.Renderables
{
    public interface IRenderable
    {
        bool Visible { get; set; }
        void Draw(SpriteBatch spriteBatch);
    }
}
