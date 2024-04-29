using ProjectGaem2.Engine.Physics.Shapes;

namespace ProjectGaem2.Engine.ECS.Components.Physics.Colliders
{
    public class BoxCollider : Collider
    {
        public BoxCollider()
            : base()
        {
            Shape = new Box2D();
        }

        public BoxCollider(float width, float height)
            : base()
        {
            Shape = new Box2D(width, height);
        }
    }
}
