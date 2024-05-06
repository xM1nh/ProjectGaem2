using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Graphics;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Physics.Shapes;

namespace ProjectGaem2.Engine.ECS.Components.Physics.Colliders
{
    public class BoxCollider : Collider
    {
        public float Width
        {
            get => ((Box2D)Shape).Width;
            set
            {
                _autoSizing = false;
                var box = (Box2D)Shape;

                if (value != box.Width)
                {
                    _isDirty = true;
                    box.Width = value;
                    if (Entity is not null && Enable)
                    {
                        PhysicsSystem.UpdateCollider(this);
                    }
                }
            }
        }

        public float Height
        {
            get => ((Box2D)Shape).Height;
            set
            {
                _autoSizing = false;
                var box = (Box2D)Shape;

                if (value != box.Height)
                {
                    box.Height = value;
                    _isDirty = true;

                    if (Entity is not null && Enable)
                    {
                        PhysicsSystem.UpdateCollider(this);
                    }
                }
            }
        }

        public override Vector2 Origin => new(((Box2D)Shape).Width / 2, ((Box2D)Shape).Height / 2);

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

        public override void DebugDraw(PrimitiveBatch primitiveBatch)
        {
            primitiveBatch.DrawRectangle(
                Bounds.Location,
                new Vector2(Width, Height),
                Color.Transparent,
                Color.White
            );
        }
    }
}
