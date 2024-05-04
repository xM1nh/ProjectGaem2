using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.ECS.Components
{
    public interface IUpdatable
    {
        bool Enable { get; set; }

        void FixedUpdate();
        void Update();
    }
}
