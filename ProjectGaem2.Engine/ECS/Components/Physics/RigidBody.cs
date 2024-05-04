using System;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.ECS.Components.Physics.Colliders;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Physics.Shapes.Collisions;
using ProjectGaem2.Engine.Utils;
using ProjectGaem2.Engine.Utils.Extensions;

namespace ProjectGaem2.Engine.ECS.Components.Physics
{
    public class RigidBody : Component, IUpdatable
    {
        private Collider _collider;

        private float _mass = 10f;
        private float _inverseMass;
        private float _inertia;
        private float _inverseInertia;
        private float _restitution = 0.5f;
        private float _staticFriction = 0.5f;
        private float _dynamicFriction = 0.2f;
        private Vector2 _force = Vector2.Zero;
        private float _torque = 0f;

        public bool ShouldUseGravity = true;
        public bool IsImmovable => _mass == 0;

        public Vector2 LinearVelocity;
        public float AngularVelocity;

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

        public float Inertia
        {
            get => _inertia;
            set
            {
                _inertia = MathHelper.Clamp(value, 0, float.MaxValue);

                if (_inertia != 0)
                {
                    _inverseInertia = 1 / _inertia;
                }
                else
                {
                    _inverseInertia = 0;
                }
            }
        }

        public float Restitution
        {
            get => _restitution;
            set { _restitution = MathHelper.Clamp(value, 0, 1); }
        }

        public float StaticFriction
        {
            get => _staticFriction;
            set { _staticFriction = MathHelper.Clamp(value, 0, 1); }
        }

        public float DynamicFriction
        {
            get => _dynamicFriction;
            set { _dynamicFriction = MathHelper.Clamp(value, 0, 1); }
        }

        public RigidBody()
        {
            _inverseMass = 1 / _mass;
            _inverseInertia = 1 / _inertia;
        }

        public override void OnAddedToEntity()
        {
            _collider = Entity.GetComponent<Collider>();

            if (_collider is not null)
            {
                switch (_collider)
                {
                    case CircleCollider circle:
                        Inertia = 0.5f * _mass * circle.Radius * circle.Radius;
                        break;
                    case BoxCollider box:
                        Inertia =
                            1f / 12 * _mass * (box.Width * box.Width + box.Height * box.Height);
                        break;
                }
            }
        }

        public void Update() { }

        public void FixedUpdate()
        {
            if (IsImmovable || _collider is null)
            {
                LinearVelocity = Vector2.Zero;
                AngularVelocity = 0;

                return;
            }

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

                    Vector2 relativeVelocity;

                    var e = MathF.Min(Restitution, neighborRigidBody.Restitution);
                    var ra = manifold.ContactPoints[0] - _collider.Origin;
                    var rb = manifold.ContactPoints[0] - neighborRigidBody._collider.Origin;

                    if (neighborRigidBody is not null)
                    {
                        ProcessOverlap(neighborRigidBody, manifold.MinimumTranslationVector);
                        relativeVelocity =
                            (
                                neighborRigidBody.LinearVelocity
                                + Vector2Ext.Cross(neighborRigidBody.AngularVelocity, rb)
                            ) - (LinearVelocity + Vector2Ext.Cross(AngularVelocity, ra));
                    }
                    else
                    {
                        Entity.Transform.Position -= manifold.MinimumTranslationVector;
                        relativeVelocity = LinearVelocity + Vector2Ext.Cross(AngularVelocity, ra);
                    }

                    Vector2.Dot(
                        ref relativeVelocity,
                        ref manifold.Normal,
                        out float contactVelocityMag
                    );

                    if (contactVelocityMag > 0.0f)
                    {
                        continue;
                    }

                    var raCrossN = Vector2Ext.Cross(ra, manifold.Normal);
                    var rbCrossN = Vector2Ext.Cross(rb, manifold.Normal);
                    var invMassSum =
                        _inverseMass
                        + neighborRigidBody._inverseMass
                        + raCrossN * raCrossN * _inverseInertia
                        + rbCrossN * rbCrossN * neighborRigidBody._inverseInertia;

                    var j = -(1.0f + e) * contactVelocityMag;
                    j /= invMassSum;
                    j /= manifold.Count;
                    var impulse = j * manifold.Normal;

                    ApplyImpulse(this, -impulse, ra);
                    ApplyImpulse(neighborRigidBody, impulse, rb);

                    //Tangential Impulse
                    if (neighborRigidBody is not null)
                    {
                        relativeVelocity =
                            neighborRigidBody.LinearVelocity
                            + Vector2Ext.Cross(neighborRigidBody.AngularVelocity, rb)
                            - (LinearVelocity + Vector2Ext.Cross(AngularVelocity, ra));
                    }
                    else
                    {
                        relativeVelocity = LinearVelocity + Vector2Ext.Cross(AngularVelocity, ra);
                    }

                    var sf = MathF.Sqrt(_staticFriction * neighborRigidBody._staticFriction);
                    var df = MathF.Sqrt(_dynamicFriction * neighborRigidBody._dynamicFriction);

                    var tangent =
                        relativeVelocity
                        - Vector2.Dot(relativeVelocity, manifold.Normal) * manifold.Normal;
                    tangent.Normalize();

                    var jt = -Vector2.Dot(relativeVelocity, tangent);
                    jt /= invMassSum;
                    jt /= manifold.Count;

                    if (jt < 0.005f)
                    {
                        continue;
                    }

                    Vector2 tangentImpulse;
                    if (MathF.Abs(jt) < j * sf)
                    {
                        tangentImpulse = jt * tangent;
                    }
                    else
                    {
                        tangentImpulse = -j * tangent * df;
                    }

                    ApplyImpulse(this, -tangentImpulse, ra);
                    ApplyImpulse(neighborRigidBody, tangentImpulse, rb);

                    //Correction
                    var kSlop = 0.05f;
                    var percent = 0.4f;
                    var correction =
                        (
                            MathF.Max(manifold.Depths[0] - kSlop, 0f)
                            / (_inverseMass + neighborRigidBody._inverseMass)
                        )
                        * manifold.Normal
                        * percent;

                    Entity.Position -= correction * _inverseMass;
                    neighborRigidBody.Entity.Position +=
                        correction * neighborRigidBody._inverseMass;
                }
            }

            IntegrateForce();
            IntegrateVelocity();
        }

        void IntegrateForce()
        {
            if (IsImmovable || _collider is null)
            {
                return;
            }

            if (ShouldUseGravity)
            {
                LinearVelocity +=
                    (_force * _inverseMass + PhysicsSystem.Gravity) * (Time.DeltaTime / 2);
            }
            else
            {
                LinearVelocity += (_force * _inverseMass) * (Time.DeltaTime / 2);
            }

            AngularVelocity += _torque * _inverseInertia * (Time.DeltaTime / 2);
        }

        void IntegrateVelocity()
        {
            if (IsImmovable || _collider is null)
            {
                return;
            }

            Entity.Position += LinearVelocity * Time.DeltaTime;
            Entity.Rotation += AngularVelocity * Time.DeltaTime;

            IntegrateForce();
        }

        void ApplyImpulse(RigidBody body, in Vector2 impulse, in Vector2 contactVector)
        {
            body.LinearVelocity += impulse * body._inverseMass;
            body.AngularVelocity += Vector2Ext.Cross(contactVector, impulse) * body._inverseInertia;
        }

        void ProcessOverlap(RigidBody other, in Vector2 minimumTranslationVector)
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
