using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine.ECS;
using ProjectGaem2.Engine.ECS.Components.Physics;
using ProjectGaem2.Engine.ECS.Components.Physics.Colliders;
using ProjectGaem2.Engine.ECS.Components.Renderables;

namespace ProjectGaem2.Suika.Entities
{
    public class Orange : Entity
    {
        public Orange(Texture2D texture)
        {
            Position = new Vector2(400, 400);

            AddComponent(new SpriteRenderer(texture));

            var rigidBody = new RigidBody
            {
                LinearVelocity = new Vector2(0, -1),
                ShouldUseGravity = false,
                Mass = 0
            };
            AddComponent(rigidBody);

            AddComponent<CircleCollider>();
        }
    }
}
