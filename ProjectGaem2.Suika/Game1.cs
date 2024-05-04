using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine;
using ProjectGaem2.Suika.Entities;

namespace ProjectGaem2.Suika
{
    public class Game1 : Core
    {
        private Texture2D _appleTexture;
        private Texture2D _orangeTexture;
        private Apple _apple;
        private Orange _orange;

        public Game1()
            : base() { }

        protected override void Initialize()
        {
            base.Initialize();
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _appleTexture = Content.Load<Texture2D>("apple");
            _orangeTexture = Content.Load<Texture2D>("orange");
            _apple = new(_appleTexture);
            _orange = new(_orangeTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _apple.Update();
            _orange.Update();
        }

        protected override void FixedUpdate()
        {
            _apple.FixedUpdate();
            _orange.FixedUpdate();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.Begin();
            _apple.Draw(_spriteBatch);
            _orange.Draw(_spriteBatch);
            _spriteBatch.End();
        }
    }
}
