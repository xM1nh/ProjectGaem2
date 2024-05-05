using System.Collections.Generic;
using ProjectGaem2.Engine.ECS.Components.Physics.Colliders;
using ProjectGaem2.Engine.Physics;
using ProjectGaem2.Engine.Utils.DataStructures;

namespace ProjectGaem2.Engine.ECS.Components.Physics
{
    public class TriggerHandler(Entity entity)
    {
        private Entity _entity = entity;
        private HashSet<Pair<Collider>> _activeTriggers = [];
        private HashSet<Pair<Collider>> _previousTriggers = [];
        private List<ITrigger> _tempTriggerList = [];

        public void Update()
        {
            var colliders = _entity.GetComponents<Collider>();

            for (var i = 0; i < colliders.Count; i++)
            {
                var collider = colliders[i];
                if (!collider.Enable)
                {
                    continue;
                }

                var neighbors = PhysicsSystem.CollisionBroadphaseExcludingSelf(collider);

                foreach (var neighbor in neighbors)
                {
                    if (!collider.IsTrigger && !neighbor.IsTrigger)
                    {
                        continue;
                    }

                    if (collider.Overlaps(neighbor))
                    {
                        var pair = new Pair<Collider>(collider, neighbor);

                        var shouldTrigger =
                            !_activeTriggers.Contains(pair) && _previousTriggers.Contains(pair);

                        if (shouldTrigger)
                        {
                            HandleTrigger(pair, true);
                        }

                        _activeTriggers.Add(pair);
                    }
                }
            }

            HandleTriggerExit();
        }

        private void HandleTriggerExit()
        {
            _previousTriggers.ExceptWith(_activeTriggers);

            foreach (var pair in _previousTriggers)
            {
                HandleTrigger(pair, false);
            }

            _previousTriggers.Clear();
            _previousTriggers.UnionWith(_activeTriggers);
            _activeTriggers.Clear();
        }

        private void HandleTrigger(Pair<Collider> pair, bool isEntering)
        {
            pair.First.Entity.GetComponents(_tempTriggerList);

            for (var i = 0; i < _tempTriggerList.Count; i++)
            {
                if (isEntering)
                {
                    _tempTriggerList[i].OnTriggerEnter(pair.First, pair.Second);
                }
                else
                {
                    _tempTriggerList[i].OnTriggerExit(pair.First, pair.Second);
                }
            }

            _tempTriggerList.Clear();

            if (pair.Second is not null)
            {
                pair.Second.Entity.GetComponents(_tempTriggerList);

                for (var i = 0; i < _tempTriggerList.Count; i++)
                {
                    if (isEntering)
                    {
                        _tempTriggerList[i].OnTriggerEnter(pair.Second, pair.First);
                    }
                    else
                    {
                        _tempTriggerList[i].OnTriggerExit(pair.Second, pair.First);
                    }
                }

                _tempTriggerList.Clear();
            }
        }
    }
}
