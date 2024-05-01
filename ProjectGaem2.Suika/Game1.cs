using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine;
using ProjectGaem2.Suika.Entities;

namespace ProjectGaem2.Suika
{
    public class Game1 : Core
    {
        private Texture2D _texture;
        private Test _player;

        public Game1()
            : base() { }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _texture = Content.Load<Texture2D>("apple");
            _player = new(_texture);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _player.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.Begin();
            _player.Draw(_spriteBatch);
            _spriteBatch.End();
        }
    }
}
