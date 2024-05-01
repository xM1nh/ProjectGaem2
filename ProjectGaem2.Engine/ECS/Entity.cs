using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectGaem2.Engine.ECS.Components;
using ProjectGaem2.Engine.ECS.Utils;
using ProjectGaem2.Engine.Utils.Math;

namespace ProjectGaem2.Engine.ECS
{
    public abstract class Entity : IComparable<Entity>
    {
        private static uint _idGenerator;
        public readonly uint Id;
        public string Name;
        public Transform Transform { get; set; }
        public ComponentList Components { get; }

        public Transform Parent
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Transform.Parent;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Transform.Parent = value;
        }

        public Vector2 Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Transform.Position;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Transform.Position = value;
        }

        public Vector2 LocalPosition
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Transform.LocalPosition;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Transform.LocalPosition = value;
        }

        public float Rotation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Transform.Rotation;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Transform.Rotation = value;
        }

        public float LocalRotation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Transform.LocalRotation;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Transform.LocalRotation = value;
        }

        public Vector2 Scale
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Transform.Scale;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Transform.Scale = value;
        }

        public Vector2 LocalScale
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Transform.LocalScale;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Transform.LocalScale = value;
        }

        public Matrix2 LocalToWorldTransform
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Transform.LocalToWorldTransform;
        }

        public Matrix2 WorldToLocalTransform
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Transform.WorldToLocalTransform;
        }

        public Entity(string name)
        {
            Transform = new Transform(this);
            Components = new(this);
            Id = _idGenerator++;
            Name = name;
        }

        public Entity()
            : this(Guid.NewGuid().ToString()) { }

        public T AddComponent<T>(T component)
            where T : Component
        {
            component.Entity = this;
            Components.Add(component);
            return component;
        }

        public T AddComponent<T>()
            where T : Component, new()
        {
            var component = new T { Entity = this };
            Components.Add(component);
            return component;
        }

        public T GetComponent<T>()
            where T : Component => Components.Get<T>();

        public List<T> GetComponents<T>()
            where T : Component => Components.GetComponents<T>();

        public virtual void Update(GameTime gameTime)
        {
            Components.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Components.Draw(spriteBatch);
        }

        internal void OnTransformChanged()
        {
            Components.OnEntityTransformChanged();
        }

        public int CompareTo(Entity other) => Id.CompareTo(other.Id);
    }
}
