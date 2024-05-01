using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.ECS.Components.Physics.Colliders;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Physics.Shapes.Collisions;
using ProjectGaem2.Engine.Utils;

namespace ProjectGaem2.Engine.ECS.Components.Physics
{
    public class RigidBody : Component, IUpdatable
    {
        private Collider _collider;

        private float _mass = 10f;
        private float _elasticity = 0.5f;
        private float _friction = 0.5f;
        private float _glue = 0.1f;

        private float _inverseMass;

        public bool ShouldUseGravity = true;
        public bool IsImmovable => _mass > 0.0001f;

        public Vector2 Velocity { get; set; }

        public float Mass
        {
            get => _mass;
            set
            {
                _mass = MathHelper.Clamp(value, 0, float.MaxValue);

                if (_mass > 0.0001f)
                {
                    _inverseMass = 1 / _mass;
                }
                else
                {
                    _inverseMass = 0;
                }
            }
        }

        public float Elasticity
        {
            get => _elasticity;
            set { _elasticity = MathHelper.Clamp(value, 0, 1); }
        }

        public float Friction
        {
            get => _friction;
            set { _friction = MathHelper.Clamp(value, 0, 1); }
        }

        public float Glue
        {
            get => _glue;
            set { _glue = MathHelper.Clamp(value, 0, 10); }
        }

        public RigidBody()
        {
            _inverseMass = 1 / _mass;
        }

        public override void OnAddedToEntity()
        {
            _collider = Entity.GetComponent<Collider>();
        }

        public void Update(GameTime gameTime)
        {
            if (IsImmovable || _collider is null)
            {
                Velocity = Vector2.Zero;
                return;
            }

            if (ShouldUseGravity)
            {
                Velocity += PhysicsSystem.Gravity * Time.DeltaTime;
            }

            Entity.Transform.Position += Velocity * Time.DeltaTime;

            var neighbors = PhysicsSystem.CollisionBroadphaseExcludingSelf(_collider);
            foreach (var neighbor in neighbors)
            {
                if (neighbor.Entity == Entity)
                {
                    continue;
                }

                if (_collider.Collides(neighbor, out Manifold manifold))
                {
                    var neighborRigidBody = neighbor.Entity.GetComponent<RigidBody>();

                    if (neighborRigidBody is not null)
                    {
                        ProcessOverlap(neighborRigidBody, in manifold.MinimumTranslationVector);
                        ProcessCollision(neighborRigidBody, in manifold.MinimumTranslationVector);
                    }
                    else
                    {
                        Entity.Transform.Position -= manifold.MinimumTranslationVector;
                        var relativeVelocity = Velocity;
                        CalculateResponseVelocity(
                            ref relativeVelocity,
                            in manifold.MinimumTranslationVector,
                            out relativeVelocity
                        );
                        Velocity += relativeVelocity;
                    }
                }
            }
        }

        private void CalculateResponseVelocity(
            ref Vector2 relativeVelocity,
            in Vector2 minimumTranslationVector,
            out Vector2 responseVelocity
        )
        {
            var inverseMTV = minimumTranslationVector * -1f;
            Vector2.Normalize(ref inverseMTV, out Vector2 normal);

            // the velocity is decomposed along the normal of the collision and the plane of collision.
            // The elasticity will affect the response along the normal (normalVelocityComponent) and the friction will affect
            // the tangential component of the velocity (tangentialVelocityComponent)
            Vector2.Dot(ref relativeVelocity, ref normal, out float n);

            var normalVelocityComponent = normal * n;
            var tangentialVelocityComponent = relativeVelocity - normalVelocityComponent;

            if (n > 0.0f)
                normalVelocityComponent = Vector2.Zero;

            // if the squared magnitude of the tangential component is less than glue then we bump up the friction to the max
            var coefficientOfFriction = _friction;
            if (tangentialVelocityComponent.LengthSquared() < _glue)
                coefficientOfFriction = 1.01f;

            // elasticity affects the normal component of the velocity and friction affects the tangential component
            responseVelocity =
                -(1.0f + _elasticity) * normalVelocityComponent
                - coefficientOfFriction * tangentialVelocityComponent;
        }

        private void ProcessCollision(RigidBody other, in Vector2 minimumTranslationVector)
        {
            var relativeVelocity = Velocity - other.Velocity;

            CalculateResponseVelocity(
                ref relativeVelocity,
                in minimumTranslationVector,
                out relativeVelocity
            );

            var totalInverseMass = _inverseMass + other._inverseMass;
            var ourResponseFraction = _inverseMass / totalInverseMass;
            var otherResponseFraction = other._inverseMass / totalInverseMass;

            Velocity += relativeVelocity * ourResponseFraction;
            other.Velocity -= relativeVelocity * otherResponseFraction;
        }

        private void ProcessOverlap(RigidBody other, in Vector2 minimumTranslationVector)
        {
            if (IsImmovable)
            {
                other.Entity.Transform.Position += minimumTranslationVector;
            }
            else if (other.IsImmovable)
            {
                Entity.Transform.Position -= minimumTranslationVector;
            }
            else
            {
                Entity.Transform.Position -= minimumTranslationVector * 0.5f;
                other.Entity.Transform.Position += minimumTranslationVector * 0.5f;
            }
        }
    }
}
