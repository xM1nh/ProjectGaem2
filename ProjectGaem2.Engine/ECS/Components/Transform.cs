using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.ECS.Components
{
    public class Transform
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }
    }
}
