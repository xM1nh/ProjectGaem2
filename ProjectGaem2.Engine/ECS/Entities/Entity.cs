using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine.ECS.Components;
using ProjectGaem2.Engine.ECS.Utils;

namespace ProjectGaem2.Engine.ECS.Entities
{
    public abstract class Entity
    {
        public Transform Transform { get; set; }
        public ComponentList Components { get; }

        public Entity()
        {
            Transform = new Transform() { Entity = this };
            Components = new(this);
        }

        public T AddComponent<T>(T component)
            where T : Component
        {
            component.Entity = this;
            Components.Add(component);
            return component;
        }

        public T AddComponent<T>()
            where T : Component, new()
        {
            var component = new T { Entity = this };
            Components.Add(component);
            return component;
        }

        public T GetComponent<T>()
            where T : Component => Components.Get<T>();

        public List<T> GetComponents<T>()
            where T : Component => Components.GetComponents<T>();

        public virtual void Update(GameTime gameTime)
        {
            Components.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Components.Draw(spriteBatch);
        }
    }
}
