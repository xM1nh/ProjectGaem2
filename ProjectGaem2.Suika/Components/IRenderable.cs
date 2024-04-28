using Microsoft.Xna.Framework.Graphics;

namespace ProjectGaem2.Suika.Components
{
    public interface IRenderable
    {
        bool Visible { get; set; }

        void Draw(SpriteBatch spriteBatch);
    }
}
