using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectGaem2.Engine.ECS.Components;
using ProjectGaem2.Engine.ECS.Components.Physics;
using ProjectGaem2.Engine.ECS.Components.Renderables;
using ProjectGaem2.Engine.Input.Virtual;
using ProjectGaem2.Engine.Physics.Shapes.Collisions;

namespace ProjectGaem2.Suika.Components
{
    public class Dropper : Component, IUpdatable
    {
        Mover _mover;
        SpriteRenderer _sprite;
        VirtualButton _up;
        VirtualButton _down;
        Texture2D _texture;

        public Dropper(Texture2D texture)
        {
            _texture = texture;
        }

        public override void OnAddedToEntity()
        {
            _mover = Entity.AddComponent(new Mover());

            _sprite = Entity.AddComponent(new SpriteRenderer(_texture));

            _up = new VirtualButton();
            _up.AddKeyboardKey(Keys.Up);
            _down = new VirtualButton();
            _down.AddKeyboardKey(Keys.Down);
        }

        public void Update(GameTime gameTime)
        {
            var v = Vector2.Zero;

            if (_up.Held)
            {
                v.Y -= 1;
            }
            if (_down.Held)
            {
                v.Y += 1;
            }

            var movement = v * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            _mover.CalculateMovement(ref movement, out Manifold manifold);
            _mover.ApplyMovement(movement);
        }
    }
}
