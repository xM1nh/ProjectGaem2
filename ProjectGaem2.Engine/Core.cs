using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine.Input;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Utils;

namespace ProjectGaem2.Engine
{
    public class Core : Game
    {
        protected GraphicsDeviceManager _graphics;
        protected SpriteBatch _spriteBatch;

        private float previousT = 0;
        private float accumulator = 0.0f;
        private float maxFrameTime = 250;

        public Core()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            PhysicsSystem.Reset();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here;
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            Time.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            InputListener.Update(gameTime);

            if (previousT == 0)
            {
                previousT = (float)gameTime.TotalGameTime.TotalMilliseconds;
            }

            float now = (float)gameTime.TotalGameTime.TotalMilliseconds;
            float frameTime = now - previousT;
            if (frameTime > maxFrameTime)
            {
                frameTime = maxFrameTime;
            }

            previousT = now;

            accumulator += frameTime;

            while (accumulator >= Time.FixedDeltaTime)
            {
                FixedUpdate();
                accumulator -= Time.FixedDeltaTime;
            }

            Time.Alpha = accumulator / Time.FixedDeltaTime;

            base.Update(gameTime);
        }

        protected virtual void FixedUpdate() { }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
