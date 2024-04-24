using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.Shapes;

namespace ProjectGaem2.Engine.Physics.RigidBody
{
    public class CapsuleBody : Body
    {
        public Vector2 Start
        {
            get => ((Capsule2D)Shape).Start;
            set => ((Capsule2D)Shape).Start = value;
        }

        public Vector2 End
        {
            get => ((Capsule2D)Shape).End;
            set => ((Capsule2D)Shape).End = value;
        }

        public float Radius
        {
            get => ((Capsule2D)Shape).Radius;
            set => ((Capsule2D)Shape).Radius = value;
        }

        public CapsuleBody()
        {
            Shape = new Capsule2D();
            Transform = Transform.Identity();
        }
    }
}
