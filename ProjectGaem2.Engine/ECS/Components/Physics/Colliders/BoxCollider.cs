using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Physics.Shapes;

namespace ProjectGaem2.Engine.ECS.Components.Physics.Colliders
{
    public class BoxCollider : Collider
    {
        public float Width
        {
            get => ((Box2D)Shape).Max.X - ((Box2D)Shape).Min.X;
            set
            {
                _autoSizing = false;
                var box = (Box2D)Shape;

                if (value != box.Max.X - box.Min.X)
                {
                    _isDirty = true;
                    box.SetWidth(value);
                    if (Entity is not null && Enable)
                    {
                        PhysicsSystem.UpdateCollider(this);
                    }
                }
            }
        }

        public float Height
        {
            get => ((Box2D)Shape).Max.Y - ((Box2D)Shape).Min.Y;
            set
            {
                _autoSizing = false;
                var box = (Box2D)Shape;
                box.SetHeight(value);
                _isDirty = true;

                if (Entity is not null && Enable)
                {
                    PhysicsSystem.UpdateCollider(this);
                }
            }
        }

        public override Vector2 Origin =>
            new(
                (((Box2D)Shape).Max.X - ((Box2D)Shape).Min.X) / 2,
                (((Box2D)Shape).Max.Y - ((Box2D)Shape).Min.Y) / 2
            );

        public BoxCollider()
        {
            _autoSizing = true;
            Shape = new Box2D();
        }

        public BoxCollider(float width, float height)
        {
            _autoSizing = true;
            Shape = new Box2D(width, height);
        }
    }
}
