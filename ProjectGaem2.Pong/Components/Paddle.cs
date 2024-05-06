using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjectGaem2.Engine.ECS.Components;
using ProjectGaem2.Engine.ECS.Components.Physics;
using ProjectGaem2.Engine.Input.Virtual;
using ProjectGaem2.Engine.Physics.Shapes.Collisions;
using ProjectGaem2.Engine.Utils;

namespace ProjectGaem2.Pong.Components
{
    public class Paddle : Component, IUpdatable
    {
        Vector2 _currentPosition;
        Vector2 _prevPosition;

        Mover _mover;
        Vector2 _velocity;
        float _speed = 10f;

        VirtualButton _upInput;
        VirtualButton _downInput;
        VirtualButton _leftInput;
        VirtualButton _rightInput;

        public override void OnAddedToEntity()
        {
            _currentPosition = Entity.Position;
            _mover = new Mover();
            Entity.AddComponent(_mover);

            SetupInput();
        }

        public void FixedUpdate()
        {
            _prevPosition = _currentPosition;
            if (_upInput.Held)
            {
                _velocity.Y = -1f;
            }
            if (_downInput.Held)
            {
                _velocity.Y = 1f;
            }
            if (_rightInput.Held)
            {
                _velocity.X = 1f;
            }
            if (_leftInput.Held)
            {
                _velocity.X = -1f;
            }

            if (_velocity != Vector2.Zero)
            {
                var movement = _velocity * _speed;
                _mover.CalculateMovement(ref movement, out Manifold manifold);

                _currentPosition += movement;
            }
        }

        public void Update()
        {
            _velocity = Vector2.Zero;
            Entity.Position = Vector2.LerpPrecise(_prevPosition, _currentPosition, Time.FixedAlpha);
        }

        void SetupInput()
        {
            _upInput = new VirtualButton();
            _upInput.AddKeyboardKey(Keys.Up);

            _downInput = new VirtualButton();
            _downInput.AddKeyboardKey(Keys.Down);

            _leftInput = new VirtualButton();
            _leftInput.AddKeyboardKey(Keys.Left);

            _rightInput = new VirtualButton();
            _rightInput.AddKeyboardKey(Keys.Right);
        }
    }
}
