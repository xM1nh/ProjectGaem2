using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics.RigidBody.Shapes;
using ProjectGaem2.Engine.Physics.Shapes;

namespace ProjectGaem2.Engine.Physics.RigidBody
{
    public class CapsuleBody : Body
    {
        public CapsuleBody(Vector2 start, Vector2 end, float radius)
        {
            Shape = new Capsule2D();
            ((Capsule2D)Shape).Start = start;
            ((Capsule2D)Shape).End = end;
            ((Capsule2D)Shape).Radius = radius;
            Transform = Transform.Identity();
        }
    }
}
