using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine.ECS.Utils;
using ProjectGaem2.Engine.Graphics;

namespace ProjectGaem2.Engine.ECS
{
    public class Scene
    {
        public EntityList Entities { get; }
        public ContentManager Content;

        public Scene(ContentManager content)
        {
            Content = content;
            Entities = new(this);
        }

        public T GetEntity<T>()
            where T : class => Entities.Get<T>();

        public Entity CreateEntity(string name)
        {
            var entity = new Entity(name);
            return AddEntity(entity);
        }

        public Entity CreateEntity(string name, Vector2 position)
        {
            var entity = new Entity(name) { Position = position };
            return AddEntity(entity);
        }

        public T AddEntity<T>(T entity)
            where T : Entity
        {
            entity.Scene = this;
            Entities.Add(entity);
            return entity;
        }

        public T AddEntity<T>()
            where T : Entity, new()
        {
            var entity = new T { Scene = this };
            Entities.Add(entity);
            return entity;
        }

        public void RemoveEntity(Entity entity)
        {
            Entities.Remove(entity);
            entity.Scene = null;
        }

        public virtual void Initialize() { }

        public virtual void Update()
        {
            Entities.Update();
        }

        public virtual void FixedUpdate()
        {
            Entities.FixedUpdate();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Entities.Draw(spriteBatch);
        }

        public virtual void DebugDraw(PrimitiveBatch primitiveBatch)
        {
            Entities.DebugDraw(primitiveBatch);
        }
    }
}
