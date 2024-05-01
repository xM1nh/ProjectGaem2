using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.Utils.Extensions;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.ECS
{
    public class Transform(Entity entity)
    {
        private Transform _parent;

        private Vector2 _position;
        private float _rotation;
        private Vector2 _scale = Vector2.One;
        private Vector2 _localPosition;
        private float _localRotation;
        private Vector2 _localScale = Vector2.One;

        private bool _dirty;
        private bool _localDirty;

        private bool _positionDirty;
        private bool _localPositionDirty;
        private bool _localRotationDirty;
        private bool _localScaleDirty;

        private Matrix2 _localTransform;
        private Matrix2 _worldTransform = Matrix2.Identity;
        private Matrix2 _worldToLocalTransform = Matrix2.Identity;

        private bool _worldToLocalDirty;

        private Matrix2 _rotationMatrix;
        private Matrix2 _translationMatrix;
        private Matrix2 _scaleMatrix;

        private List<Transform> _children = [];

        public Transform Parent
        {
            get => _parent;
            set
            {
                _parent?._children.Remove(this);
                value._children.Add(this);
                _parent = value;
                SetDirty();
            }
        }
        public Entity Entity { get; set; } = entity;

        public Vector2 Position
        {
            get
            {
                UpdateTransform();
                if (_positionDirty)
                {
                    if (Parent is null)
                    {
                        _position = _localPosition;
                    }
                    else
                    {
                        Parent.UpdateTransform();
                        Vector2Ext.Transform(_localPosition, Parent._worldTransform, out _position);
                    }

                    _positionDirty = false;
                }

                return _position;
            }
            set
            {
                _position = value;
                if (Parent is not null)
                {
                    LocalPosition = Vector2.Transform(_position, WorldToLocalTransform);
                }
                else
                {
                    LocalPosition = value;
                }
                _positionDirty = false;
            }
        }
        public Vector2 LocalPosition
        {
            get
            {
                UpdateTransform();
                return _localPosition;
            }
            set
            {
                _localPosition = value;
                _localDirty =
                    _positionDirty =
                    _localPositionDirty =
                    _localRotationDirty =
                    _localScaleDirty =
                        true;
                SetDirty();
            }
        }
        public float Rotation
        {
            get
            {
                UpdateTransform();
                return _rotation;
            }
            set
            {
                _rotation = value;
                if (Parent is not null)
                {
                    LocalRotation = Parent.Rotation * value;
                }
                else
                {
                    LocalRotation = value;
                }
            }
        }
        public float LocalRotation
        {
            get
            {
                UpdateTransform();
                return _localRotation;
            }
            set
            {
                _localRotation = value;
                _localDirty =
                    _positionDirty =
                    _localPositionDirty =
                    _localRotationDirty =
                    _localScaleDirty =
                        true;
                SetDirty();
            }
        }
        public Vector2 Scale
        {
            get
            {
                UpdateTransform();
                return _scale;
            }
            set
            {
                _scale = value;
                if (Parent is not null)
                {
                    LocalScale = value / Parent._scale;
                }
                else
                {
                    LocalScale = value;
                }
            }
        }
        public Vector2 LocalScale
        {
            get
            {
                UpdateTransform();
                return _localScale;
            }
            set
            {
                _localScale = value;
                _localDirty = _positionDirty = _localScaleDirty = true;
                SetDirty();
            }
        }

        public Matrix2 LocalToWorldTransform
        {
            get
            {
                UpdateTransform();
                return _worldTransform;
            }
        }
        public Matrix2 WorldToLocalTransform
        {
            get
            {
                if (_worldToLocalDirty)
                {
                    if (Parent == null)
                    {
                        _worldToLocalTransform = Matrix2.Identity;
                    }
                    else
                    {
                        Parent.UpdateTransform();
                        Matrix2.Invert(ref Parent._worldTransform, out _worldToLocalTransform);
                    }

                    _worldToLocalDirty = false;
                }

                return _worldToLocalTransform;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void UpdateTransform()
        {
            if (_dirty)
            {
                Parent?.UpdateTransform();

                if (_localDirty)
                {
                    if (_localPositionDirty)
                    {
                        Matrix2.CreateTranslation(
                            _localPosition.X,
                            _localPosition.Y,
                            out _translationMatrix
                        );
                        _localPositionDirty = false;
                    }

                    if (_localRotationDirty)
                    {
                        Matrix2.CreateRotation(_localRotation, out _rotationMatrix);
                        _localRotationDirty = false;
                    }

                    if (_localScaleDirty)
                    {
                        Matrix2.CreateScale(_localScale.X, _localScale.Y, out _scaleMatrix);
                        _localScaleDirty = false;
                    }

                    Matrix2.Multiply(ref _scaleMatrix, ref _rotationMatrix, out _localTransform);
                    Matrix2.Multiply(
                        ref _localTransform,
                        ref _translationMatrix,
                        out _localTransform
                    );

                    if (Parent == null)
                    {
                        _worldTransform = _localTransform;
                        _rotation = _localRotation;
                        _scale = _localScale;
                    }

                    if (Parent != null)
                    {
                        Matrix2.Multiply(
                            ref _localTransform,
                            ref Parent._worldTransform,
                            out _worldTransform
                        );

                        _rotation = _localRotation + Parent._rotation;
                        _scale = Parent._scale * _localScale;
                    }

                    _worldToLocalDirty = true;
                    _positionDirty = true;
                    _dirty = false;
                }
            }
        }

        void SetDirty()
        {
            Entity.OnTransformChanged();
            for (var i = 0; i < _children.Count; i++)
            {
                _children[i].SetDirty();
            }
        }

        public override string ToString()
        {
            return string.Format(
                "[Transform: parent: {0}, position: {1}, rotation: {2}, scale: {3}, localPosition: {4}, localRotation: {5}, localScale: {6}]",
                Parent != null,
                Position,
                Rotation,
                Scale,
                LocalPosition,
                LocalRotation,
                LocalScale
            );
        }
    }
}
