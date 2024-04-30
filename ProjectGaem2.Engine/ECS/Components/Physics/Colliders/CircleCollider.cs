using ProjectGaem2.Engine.Physics.Shapes;

namespace ProjectGaem2.Engine.ECS.Components.Physics.Colliders
{
    public class CircleCollider : Collider
    {
        public CircleCollider()
        {
            Shape = new Circle();
        }

        public CircleCollider(float radius)
        {
            Shape = new Circle(radius);
        }
    }
}
