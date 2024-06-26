﻿using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine.ECS.Components;
using ProjectGaem2.Engine.ECS.Components.Renderables;
using ProjectGaem2.Engine.Graphics;

namespace ProjectGaem2.Engine.ECS.Utils
{
    public class ComponentList(Entity entity)
    {
        Entity _entity = entity;
        List<Component> _components = [];
        List<Component> _componentsToAdd = [];
        List<Component> _componentsToRemove = [];

        List<IUpdatable> _updatableComponents = [];
        List<IRenderable> _renderableComponents = [];
        List<Component> _buffer = [];

        public int Count => _components.Count;

        public Component this[int index] => _components[index];

        public T Get<T>()
            where T : class
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i] is T)
                {
                    return _components[i] as T;
                }
            }

            return null;
        }

        public void Add(Component component)
        {
            _componentsToAdd.Add(component);
        }

        public void Remove(Component component)
        {
            _componentsToAdd.Remove(component);

            _componentsToRemove.Add(component);
        }

        public void GetComponents<T>(List<T> components)
            where T : class
        {
            for (var i = 0; i < _components.Count; i++)
            {
                if (_components[i] is T component)
                    components.Add(component);
            }

            // we also check the pending components just in case addComponent and getComponent are called in the same frame
            for (var i = 0; i < _componentsToAdd.Count; i++)
            {
                if (_componentsToAdd[i] is T component)
                    components.Add(component as T);
            }
        }

        public List<T> GetComponents<T>()
            where T : class
        {
            var components = new List<T>();
            GetComponents(components);

            return components;
        }

        void HandleUpdate()
        {
            if (_componentsToRemove.Count > 0)
            {
                for (int i = 0; i < _componentsToRemove.Count; i++)
                {
                    var component = _componentsToRemove[i];
                    if (component is IUpdatable)
                    {
                        _updatableComponents.Remove(component as IUpdatable);
                    }

                    if (component is IRenderable)
                    {
                        _renderableComponents.Remove(component as IRenderable);
                    }

                    _components.Remove(component);

                    component.OnRemovedFromEntity();
                    component.Entity = null;
                }

                _componentsToRemove.Clear();
            }

            if (_componentsToAdd.Count > 0)
            {
                for (int i = 0; i < _componentsToAdd.Count; i++)
                {
                    var component = _componentsToAdd[i];
                    if (component is IUpdatable)
                    {
                        _updatableComponents.Add(component as IUpdatable);
                    }

                    if (component is IRenderable)
                    {
                        _renderableComponents.Add(component as IRenderable);
                    }

                    _components.Add(component);
                    _buffer.Add(component);
                }

                _componentsToAdd.Clear();

                for (var i = 0; i < _buffer.Count; i++)
                {
                    var component = _buffer[i];
                    component.OnAddedToEntity();

                    if (component.Enable)
                    {
                        component.OnEnable();
                    }
                }

                _buffer.Clear();
            }
        }

        public void Update()
        {
            HandleUpdate();
            for (int i = 0; i < _updatableComponents.Count; i++)
            {
                _updatableComponents[i].Update();
            }
        }

        public void FixedUpdate()
        {
            HandleUpdate();
            for (int i = 0; i < _updatableComponents.Count; i++)
            {
                _updatableComponents[i].FixedUpdate();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            HandleUpdate();
            for (int i = 0; i < _renderableComponents.Count; i++)
            {
                _renderableComponents[i].Draw(spriteBatch);
            }
        }

        public void DebugDraw(PrimitiveBatch primitiveBatch)
        {
            HandleUpdate();
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].DebugDraw(primitiveBatch);
            }
        }

        public void OnEntityTransformChanged()
        {
            for (var i = 0; i < _components.Count; i++)
            {
                if (_components[i].Enable)
                {
                    _components[i].OnEntityTransformChanged();
                }
            }

            for (var i = 0; i < _componentsToAdd.Count; i++)
            {
                if (_componentsToAdd[i].Enable)
                {
                    _componentsToAdd[i].OnEntityTransformChanged();
                }
            }
        }
    }
}
