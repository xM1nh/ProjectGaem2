using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine.ECS;
using ProjectGaem2.Engine.ECS.Components.Physics;
using ProjectGaem2.Engine.ECS.Components.Physics.Colliders;
using ProjectGaem2.Engine.ECS.Components.Renderables;
using ProjectGaem2.Engine.Utils;
using ProjectGaem2.Pong.Components;

namespace ProjectGaem2.Pong.Scenes
{
    public class MainScene(ContentManager content) : Scene(content)
    {
        public override void Initialize()
        {
            var ballEntity = CreateEntity("ball", Screen.Center);
            ballEntity.AddComponent(new Ball());

            var p1Texture = Content.Load<Texture2D>("Player");
            var p1Entity = CreateEntity(
                "player",
                new Vector2(Screen.Center.X + Screen.Width - 450, Screen.Center.Y)
            );
            p1Entity.AddComponent(new Paddle());
            p1Entity.AddComponent(new SpriteRenderer(p1Texture));
            p1Entity.AddComponent<BoxCollider>();

            var p2Texture = Content.Load<Texture2D>("Computer");
            var p2Entity = CreateEntity(
                "enemy",
                new Vector2(Screen.Center.X - Screen.Width + 450, Screen.Center.Y)
            );
            p2Entity.AddComponent(new EnemyPaddle());
            p2Entity.AddComponent(new SpriteRenderer(p2Texture));
            p2Entity.AddComponent<BoxCollider>();

            var ceilEntity = CreateEntity("ceil", new Vector2(Screen.Width / 2, 0));
            ceilEntity.AddComponent(new BoxCollider() { Width = Screen.Width });
            ceilEntity.AddComponent(
                new RigidBody()
                {
                    Static = true,
                    Restitution = 1,
                    StaticFriction = 0,
                    DynamicFriction = 0
                }
            );

            var floorEntity = CreateEntity("floor", new Vector2(Screen.Width / 2, Screen.Height));
            floorEntity.AddComponent(new BoxCollider() { Width = Screen.Width });
            floorEntity.AddComponent(
                new RigidBody()
                {
                    Static = true,
                    Restitution = 1,
                    StaticFriction = 0,
                    DynamicFriction = 0
                }
            );
        }
    }
}
