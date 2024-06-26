﻿using System;
using System.Threading;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.ECS.Components.Renderables;
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
        protected bool _autoSizing;
        public bool IsTrigger;

        public event Action Collided;
        public event Action Overlaped;

        //private Timer _collisionEventDebounceTimer;
        //private readonly object _collisionEventTimerLock = new();
        //private int _collisionEventDueTime = 50;

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

        public virtual Vector2 Origin { get; }

        public Collider()
        {
            //_collisionEventDebounceTimer = new Timer(
            //    OnCollisionDebounceTimerElapsed,
            //    null,
            //    Timeout.Infinite,
            //    Timeout.Infinite
            //);
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

        public bool Collides(Collider other, out Manifold manifold)
        {
            var didCollide = Shape.Collides(other.Shape, out manifold);
            if (didCollide)
            {
                //lock (_collisionEventTimerLock)
                //{
                //    _collisionEventDebounceTimer.Change(_collisionEventDueTime, Timeout.Infinite);
                //}
                Collided?.Invoke();
            }

            return didCollide;
        }

        public bool Collides(Collider other, Vector2 motion, out Manifold manifold)
        {
            var oldPosition = Entity.Position;
            Shape.Transform.Position = Entity.Position + motion;

            var didCollide = Shape.Collides(other.Shape, out manifold);
            if (didCollide)
            {
                //lock (_collisionEventTimerLock)
                //{
                //    _collisionEventDebounceTimer.Change(_collisionEventDueTime, Timeout.Infinite);
                //}
                Collided?.Invoke();
            }

            Shape.Transform.Position = oldPosition;
            return didCollide;
        }

        public bool Overlaps(Collider other)
        {
            var didOverlap = Shape.Overlaps(other.Shape);
            if (didOverlap)
            {
                //lock (_collisionEventTimerLock)
                //{
                //    _collisionEventDebounceTimer.Change(_collisionEventDueTime, Timeout.Infinite);
                //}
                Overlaped?.Invoke();
            }

            return didOverlap;
        }

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
            if (_autoSizing)
            {
                var renderable = Entity.GetComponent<RenderableComponent>();

                if (renderable is not null)
                {
                    var bounds = renderable.Bounds;

                    var width = bounds.Width / Entity.Scale;
                    var height = bounds.Height / Entity.Scale;

                    if (this is CircleCollider circle)
                    {
                        circle.Radius = MathF.Max(width, height) * 0.5f;

                        LocalOffset = bounds.Center - Entity.Position;
                    }
                    else
                    {
                        var box = this as BoxCollider;
                        box.Width = width;
                        box.Height = height;

                        LocalOffset = bounds.Center - Entity.Position;
                    }
                }
            }

            Shape.SetTransform(Entity.Position, Entity.Rotation);
            Shape.CalculateBounds();

            if (_isRegistered)
            {
                PhysicsSystem.UpdateCollider(this);
            }

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
            Shape.SetTransform(Entity.Position, Entity.Rotation);

            if (_isRegistered)
            {
                PhysicsSystem.UpdateCollider(this);
            }
        }

        private void OnCollisionDebounceTimerElapsed(object state)
        {
            Collided?.Invoke();
            Overlaped?.Invoke();
        }
    }
}
