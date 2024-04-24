﻿using Microsoft.Xna.Framework;

namespace ProjectGaem2.Engine.Physics.RigidBody.Shapes.Collisions
{
    public struct Manifold
    {
        public int Count;
        public float[] Depths;
        public Vector2[] ContactPoints;
        public Vector2 Normal;

        public Manifold()
        {
            Depths = new float[2];
            ContactPoints = new Vector2[2];
        }
    }
}