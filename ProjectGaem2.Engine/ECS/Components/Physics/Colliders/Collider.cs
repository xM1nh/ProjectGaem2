using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Physics.Shapes;
using ProjectGaem2.Engine.Physics.Shapes.Collisions;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.ECS.Components.Physics.Colliders
{
    public class Collider : Component
    {
        protected bool _isDirty;
        internal RectangleF RegisteredBounds;
        protected Vector2 _localOffset;
        protected bool _isRegistered;

        public Shape Shape { get; protected set; }

        public RectangleF Bounds
        {
            get
            {
                if (_isDirty)
                {
                    Shape.CalculateBounds();
                    _isDirty = false;
                }

                return Shape.Bounds;
            }
        }

        public Vector2 LocalOffset
        {
            get => _localOffset;
            set
            {
                if (Enable)
                {
                    UnregisterWithPhysicsSystem();
                }
                _localOffset = value;
                _isDirty = true;
                if (Enable)
                {
                    RegisterWithPhysicsSystem();
                }
            }
        }

        public virtual void RegisterWithPhysicsSystem()
        {
            if (!_isRegistered && Enable)
            {
                PhysicsSystem.AddCollider(this);
                _isRegistered = true;
            }
        }

        public virtual void UnregisterWithPhysicsSystem()
        {
            if (_isRegistered)
            {
                PhysicsSystem.RemoveCollider(this);
            }
            _isRegistered = false;
        }

        public Vector2 AbsolutePosition => Entity.Position + _localOffset;

        public bool Collides(Collider other, out Manifold manifold) =>
            Shape.Collides(other.Shape, out manifold);

        public bool Collides(Collider other, Vector2 motion, out Manifold manifold)
        {
            var oldPosition = Entity.Transform.Position;
            Shape.Transform.Position = Entity.Transform.Position + motion;
            var didCollide = Shape.Collides(other.Shape, out manifold);
            Shape.Transform.Position = oldPosition;
            return didCollide;
        }

        public bool Overlaps(Collider other) => Shape.Overlaps(other.Shape);

        public override void OnEnable()
        {
            RegisterWithPhysicsSystem();
            _isDirty = true;
        }

        public override void OnDisable()
        {
            UnregisterWithPhysicsSystem();
        }

        public override void OnAddedToEntity()
        {
            if (Enable)
            {
                RegisterWithPhysicsSystem();
            }
        }

        public override void OnRemovedFromEntity()
        {
            UnregisterWithPhysicsSystem();
        }

        public override void OnEntityTransformChanged()
        {
            _isDirty = true;
            Shape.SetTransform(AbsolutePosition, Entity.Rotation);
        }
    }
}
