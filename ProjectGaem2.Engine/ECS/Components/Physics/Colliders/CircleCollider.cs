using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Graphics;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Physics.Shapes;

namespace ProjectGaem2.Engine.ECS.Components.Physics.Colliders
{
    public class CircleCollider : Collider
    {
        public float Radius
        {
            get => ((Circle)Shape).Radius;
            set
            {
                var circle = (Circle)Shape;
                if (value != circle.Radius)
                {
                    circle.Radius = value;
                    _isDirty = true;

                    if (Entity is not null && Enable)
                    {
                        PhysicsSystem.UpdateCollider(this);
                    }
                }
            }
        }

        public override Vector2 Origin => ((Circle)Shape).Center;

        public CircleCollider()
            : base()
        {
            _autoSizing = true;
            Shape = new Circle();
        }

        public CircleCollider(float radius)
            : base()
        {
            _autoSizing = true;
            Shape = new Circle(radius);
        }

        public override void DebugDraw(PrimitiveBatch primitiveBatch)
        {
            primitiveBatch.DrawRectangle(
                Bounds.Location,
                new Vector2(Radius * 2, Radius * 2),
                Color.Transparent,
                Color.White
            );
        }
    }
}
