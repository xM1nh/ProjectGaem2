using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine.ECS;
using ProjectGaem2.Engine.ECS.Components.Physics;
using ProjectGaem2.Engine.ECS.Components.Physics.Colliders;
using ProjectGaem2.Engine.ECS.Components.Renderables;

namespace ProjectGaem2.Suika.Entities
{
    public class Apple : Entity
    {
        public Apple(Texture2D texture)
        {
            Position = new Vector2(400, 0);

            AddComponent(new SpriteRenderer(texture));

            var rigidBody = new RigidBody
            {
                LinearVelocity = new Vector2(0, 5),
                ShouldUseGravity = false
            };
            AddComponent(rigidBody);

            AddComponent<CircleCollider>();
        }
    }
}
