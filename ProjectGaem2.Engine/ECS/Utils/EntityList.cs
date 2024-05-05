using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectGaem2.Engine.ECS.Utils
{
    public class EntityList(Scene scene)
    {
        Scene _scene = scene;
        List<Entity> _entities = [];
        HashSet<Entity> _entitiesToAdd = [];
        HashSet<Entity> _entitiesToRemove = [];

        public T Get<T>()
            where T : class
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                if (_entities[i] is T)
                {
                    return _entities[i] as T;
                }
            }

            return null;
        }

        public void Add(Entity entity)
        {
            _entitiesToAdd.Add(entity);
        }

        public List<Entity> GetAll() => _entities;

        public void Remove(Entity entity)
        {
            _entitiesToAdd.Remove(entity);

            _entitiesToRemove.Add(entity);
        }

        void HandleUpdate()
        {
            if (_entitiesToRemove.Count > 0)
            {
                foreach (var entity in _entitiesToRemove)
                {
                    _entities.Remove(entity);
                    entity.Scene = null;
                }

                _entitiesToRemove.Clear();
            }

            if (_entitiesToAdd.Count > 0)
            {
                foreach (var entity in _entitiesToAdd)
                {
                    _entities.Add(entity);
                    entity.Scene = _scene;
                }

                _entitiesToAdd.Clear();
            }
        }

        public void Update()
        {
            HandleUpdate();
            for (int i = 0; i < _entities.Count; i++)
            {
                _entities[i].Update();
            }
        }

        public void FixedUpdate()
        {
            HandleUpdate();
            for (int i = 0; i < _entities.Count; i++)
            {
                _entities[i].FixedUpdate();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            HandleUpdate();
            for (int i = 0; i < _entities.Count; i++)
            {
                _entities[i].Draw(spriteBatch);
            }
        }
    }
}
