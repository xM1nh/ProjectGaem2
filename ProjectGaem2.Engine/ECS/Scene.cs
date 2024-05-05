using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine.ECS.Utils;

namespace ProjectGaem2.Engine.ECS
{
    public class Scene
    {
        public EntityList Entities { get; }
        protected ContentManager _content;

        public Scene(ContentManager content)
        {
            _content = content;
            Entities = new(this);
        }

        public T GetEntity<T>()
            where T : class => Entities.Get<T>();

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
    }
}
