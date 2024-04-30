using System;
using ProjectGaem2.Engine.ECS.Entities;

namespace ProjectGaem2.Engine.ECS.Components
{
    public class Component
    {
        private bool _enable = true;
        public Entity Entity { get; set; }
        public bool Enable
        {
            get => _enable;
            set
            {
                _enable = value;
                if (_enable)
                {
                    OnEnable();
                }
                else
                {
                    OnDisable();
                }
            }
        }

        public virtual void OnEnable() { }

        public virtual void OnDisable() { }

        public virtual void OnAddedToEntity() { }

        public virtual void OnRemovedFromEntity() { }

        public virtual void OnEntityTransformChanged() { }
    }
}
