using Microsoft.Xna.Framework;
using ProjectGaem2.Engine.ECS.Components.Physics.Colliders;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Physics.Shapes.Collisions;

namespace ProjectGaem2.Engine.ECS.Components.Physics
{
    public class Mover : Component
    {
        TriggerHandler _triggerHandler;

        public override void OnAddedToEntity()
        {
            _triggerHandler = new TriggerHandler(Entity);
        }

        public bool CalculateMovement(ref Vector2 motion, out Manifold manifold)
        {
            manifold = new Manifold();

            // no collider? just move and forget about it
            if (Entity.GetComponent<Collider>() is null || _triggerHandler is null)
                return false;

            // 1. move all non-trigger Colliders and get closest collision
            var colliders = Entity.GetComponents<Collider>();
            for (var i = 0; i < colliders.Count; i++)
            {
                var collider = colliders[i];

                if (collider.IsTrigger)
                {
                    continue;
                }

                // fetch anything that we might collide with at our new position
                var bounds = collider.Bounds;
                bounds.X += motion.X;
                bounds.Y += motion.Y;
                var neighbors = PhysicsSystem.CollisionBroadphaseExcludingSelf(collider);

                foreach (var neighbor in neighbors)
                {
                    if (neighbor.IsTrigger)
                    {
                        continue;
                    }

                    if (collider.Collides(neighbor, motion, out Manifold _InternalManifold))
                    {
                        // hit. back off our motion
                        motion -= _InternalManifold.Depths[0] * _InternalManifold.Normal;

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
            Entity.Position += motion;

            _triggerHandler?.Update();
        }

        public bool Move(Vector2 motion, out Manifold manifold)
        {
            CalculateMovement(ref motion, out manifold);

            ApplyMovement(motion);

            return manifold.Count != 0;
        }
    }
}
