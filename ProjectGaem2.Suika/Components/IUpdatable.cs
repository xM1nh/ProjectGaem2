using Microsoft.Xna.Framework;

namespace ProjectGaem2.Suika.Components
{
    public interface IUpdatable
    {
        bool Enable { get; set; }
        void Update();
    }
}
