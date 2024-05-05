using ProjectGaem2.Engine.ECS.Components.Physics.Colliders;

namespace ProjectGaem2.Engine.ECS.Components.Physics
{
    public interface ITrigger
    {
        void OnTriggerEnter(Collider self, Collider other);
        void OnTriggerExit(Collider self, Collider other);
    }
}
