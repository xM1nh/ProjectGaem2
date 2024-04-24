using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.Shapes;

namespace ProjectGaem2.Engine.Physics.RigidBody
{
    public class CircleBody : Body
    {
        public float Radius
        {
            get => ((Circle)Shape).Radius;
            set => ((Circle)Shape).Radius = value;
        }

        public Vector2 Position
        {
            get => ((Circle)Shape).Center;
            set => ((Circle)Shape).Center = value;
        }

        public CircleBody()
        {
            Shape = new Circle();
            Transform = Transform.Identity();
        }
    }
}
