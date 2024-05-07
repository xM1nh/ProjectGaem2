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

        //For linear interpolation
        private Vector2 _currentPosition;
        private Vector2 _prevPosition;
        private float _currentAngle;
        private float _prevAngle;

        private float _mass = 10f;
        private float _inverseMass;
        private float _inertia;
        private float _inverseInertia;
        private float _restitution = 0.5f;
        private float _staticFriction = 0.5f;
        private float _dynamicFriction = 0.3f;
        private Vector2 _force = Vector2.Zero;
        private float _torque = 0f;

        private Vector2 _linearVelocity = Vector2.Zero;
        private float _angularVelocity = 0f;
        private float _maxLinearVelocity = 100;
        private float _maxAngularVelocity = 7;

        public bool ShouldUseGravity = true;
        public bool Static
        {
            get => _mass == 0;
            set
            {
                if (value)
                {
                    _inertia = 0;
                    _inverseInertia = 0;
                    _mass = 0;
                    _inverseMass = 0;
                }
            }
        }

        public Vector2 LinearVelocity
        {
            get => _linearVelocity;
            set
            {
                var x = MathHelper.Clamp(value.X, -_maxLinearVelocity, _maxLinearVelocity);
                var y = MathHelper.Clamp(value.Y, -_maxLinearVelocity, _maxLinearVelocity);
                _linearVelocity = new Vector2(x, y);
            }
        }
        public float AngularVelocity
        {
            get => _angularVelocity;
            set
            {
                _angularVelocity = MathHelper.Clamp(
                    value,
                    -_maxAngularVelocity,
                    _maxAngularVelocity
                );
            }
        }

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

            if (!Static || _collider is not null)
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

            _currentPosition = Entity.Position;
            _currentAngle = Entity.Rotation;
        }

        public void Update()
        {
            if (Static || _collider is null)
            {
                LinearVelocity = Vector2.Zero;
                AngularVelocity = 0;

                return;
            }

            Entity.Position = Vector2.LerpPrecise(_prevPosition, _currentPosition, Time.Alpha);
            Entity.Rotation = MathHelper.LerpPrecise(_prevAngle, _currentAngle, Time.Alpha);
        }

        public void FixedUpdate()
        {
            if (Static || _collider is null)
            {
                LinearVelocity = Vector2.Zero;
                AngularVelocity = 0;

                return;
            }

            IntegrateForce();

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

                    var ra = manifold.ContactPoints[0] - _collider.Origin;
                    var rb = manifold.ContactPoints[0] - neighbor.Origin;

                    float oim;
                    float oii;
                    float odf;
                    float osf;
                    float e;
                    Vector2 relativeVelocity;

                    if (neighborRigidBody is not null)
                    {
                        ProcessOverlap(neighborRigidBody, manifold.Depths[0] * manifold.Normal);

                        oim = neighborRigidBody._inverseMass;
                        oii = neighborRigidBody._inverseInertia;
                        odf = neighborRigidBody._dynamicFriction;
                        osf = neighborRigidBody._staticFriction;
                        e = MathF.Min(Restitution, neighborRigidBody.Restitution);

                        relativeVelocity =
                            (
                                neighborRigidBody.LinearVelocity
                                + Vector2Ext.Cross(neighborRigidBody.AngularVelocity, rb)
                            ) - (LinearVelocity + Vector2Ext.Cross(AngularVelocity, ra));
                    }
                    else
                    {
                        Entity.Position -= manifold.Depths[0] * manifold.Normal;
                        _prevPosition = _currentPosition;
                        _currentPosition = Entity.Position;

                        oim = 0;
                        oii = 0;
                        odf = 0;
                        osf = 0;
                        e = Restitution;

                        relativeVelocity = -(
                            LinearVelocity + Vector2Ext.Cross(AngularVelocity, ra)
                        );
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
                        + oim
                        + raCrossN * raCrossN * _inverseInertia
                        + rbCrossN * rbCrossN * oii;

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
                        relativeVelocity = -(
                            LinearVelocity + Vector2Ext.Cross(AngularVelocity, ra)
                        );
                    }

                    var sf = MathF.Sqrt(_staticFriction * osf);
                    var df = MathF.Sqrt(_dynamicFriction * odf);

                    var tangent =
                        relativeVelocity
                        - Vector2.Dot(relativeVelocity, manifold.Normal) * manifold.Normal;

                    if (Vector2Ext.Equal(tangent, Vector2.Zero))
                    {
                        continue;
                    }
                    else
                    {
                        tangent.Normalize();
                    }

                    var jt = -Vector2.Dot(relativeVelocity, tangent);
                    jt /= invMassSum;
                    jt /= manifold.Count;

                    if (Equals(jt, 0))
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
                        (MathF.Max(manifold.Depths[0] - kSlop, 0f) / (_inverseMass + oim))
                        * manifold.Normal
                        * percent;

                    Entity.Position -= correction * _inverseMass;
                    if (neighborRigidBody is not null && !neighborRigidBody.Static)
                        neighborRigidBody.Entity.Position +=
                            correction * neighborRigidBody._inverseMass;
                }
            }

            IntegrateVelocity();

            _force = Vector2.Zero;
            _torque = 0;
        }

        void IntegrateForce()
        {
            if (Static || _collider is null)
            {
                return;
            }

            if (ShouldUseGravity)
            {
                LinearVelocity += (_force * _inverseMass + PhysicsSystem.Gravity) * 0.5f;
            }
            else
            {
                LinearVelocity += (_force * _inverseMass) * 0.5f;
            }

            AngularVelocity += _torque * _inverseInertia * 0.5f;
        }

        void IntegrateVelocity()
        {
            if (Static || _collider is null)
            {
                return;
            }

            _prevPosition = _currentPosition;
            _prevAngle = _currentAngle;

            _currentPosition += LinearVelocity;
            _currentAngle += AngularVelocity;

            IntegrateForce();
        }

        void ApplyImpulse(RigidBody body, in Vector2 impulse, in Vector2 contactVector)
        {
            if (body is not null)
            {
                LinearVelocity += impulse * body._inverseMass;
                AngularVelocity += Vector2Ext.Cross(contactVector, impulse) * body._inverseInertia;
            }
        }

        void ProcessOverlap(RigidBody other, in Vector2 minimumTranslationVector)
        {
            if (Static)
            {
                other.Entity.Position += minimumTranslationVector;
                other._prevPosition = other._currentPosition;
                other._currentPosition = other.Entity.Position;
            }
            else if (other.Static)
            {
                Entity.Position -= minimumTranslationVector;
                _prevPosition = _currentPosition;
                _currentPosition = Entity.Position;
            }
            else
            {
                Entity.Position -= minimumTranslationVector * 0.5f;
                _prevPosition = _currentPosition;
                _currentPosition = Entity.Position;

                other.Entity.Position += minimumTranslationVector * 0.5f;
                other._prevPosition = other._currentPosition;
                other._currentPosition = other.Entity.Position;
            }
        }

        public void AddForce(Vector2 force)
        {
            if (!Static)
            {
                _force = force;
            }
        }

        public void AddTorque(float torque)
        {
            if (!Static)
            {
                _torque = torque;
            }
        }
    }
}
