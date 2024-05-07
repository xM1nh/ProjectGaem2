using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine.ECS.Components;
using ProjectGaem2.Engine.ECS.Components.Physics;
using ProjectGaem2.Engine.ECS.Components.Physics.Colliders;
using ProjectGaem2.Engine.ECS.Components.Renderables;

namespace ProjectGaem2.Pong.Components
{
    public class Ball : Component
    {
        CircleCollider _collider;
        SoundEffect _sfx;

        public override void OnAddedToEntity()
        {
            _sfx = Entity.Scene.Content.Load<SoundEffect>("hit(1)");

            var texture = Entity.Scene.Content.Load<Texture2D>("Ball");
            Entity.AddComponent(new SpriteRenderer(texture));

            _collider = Entity.AddComponent<CircleCollider>();
            _collider.Collided += OnCollided;

            Entity.AddComponent(
                new RigidBody()
                {
                    ShouldUseGravity = false,
                    LinearVelocity = new Vector2(5, 0),
                    Restitution = 1,
                    StaticFriction = 0,
                    DynamicFriction = 0,
                }
            );
        }

        void OnCollided()
        {
            _sfx.Play();
        }
    }
}
