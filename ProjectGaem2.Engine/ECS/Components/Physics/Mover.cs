using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.ECS.Components.Physics.Colliders;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Physics.Shapes.Collisions;

namespace ProjectGaem2.Engine.ECS.Components.Physics
{
    public class Mover : Component
    {
        public bool CalculateMovement(ref Vector2 motion, out Manifold manifold)
        {
            manifold = new Manifold();

            // no collider? just move and forget about it
            if (Entity.GetComponent<Collider>() == null)
                return false;

            // 1. move all non-trigger Colliders and get closest collision
            var colliders = Entity.GetComponents<Collider>();
            for (var i = 0; i < colliders.Count; i++)
            {
                var collider = colliders[i];

                // fetch anything that we might collide with at our new position
                var bounds = collider.Bounds;
                bounds.X += motion.X;
                bounds.Y += motion.Y;
                var neighbors = PhysicsSystem.CollisionBroadphaseExcludingSelf(collider);

                foreach (var neighbor in neighbors)
                {
                    if (collider.Collides(neighbor, motion, out Manifold _InternalManifold))
                    {
                        // hit. back off our motion
                        motion -= _InternalManifold.MinimumTranslationVector;

                        // If we hit multiple objects, only take on the first for simplicity sake.
                        if (_InternalManifold.Count != 0)
                            manifold = _InternalManifold;
                    }
                }
            }

            return manifold.Count != 0;
        }

        public void ApplyMovement(Vector2 motion)
        {
            Entity.Transform.Position += motion;
        }

        public bool Move(Vector2 motion, out Manifold manifold)
        {
            CalculateMovement(ref motion, out manifold);

            ApplyMovement(motion);

            return manifold.Count != 0;
        }
    }
}
