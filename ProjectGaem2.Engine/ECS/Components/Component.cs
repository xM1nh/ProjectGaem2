using ProjectGaem2.Engine.ECS.Entities;

namespace ProjectGaem2.Engine.ECS.Components
{
    public class Component
    {
        public Entity Entity { get; set; }
        public bool Enable { get; set; }

        public virtual void OnAddedToEntity() { }
    }
}
