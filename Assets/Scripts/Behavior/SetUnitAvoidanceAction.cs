using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

namespace Oko.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SetUnitAvoidance", story: "Set [Unit] avoidance quality to [AvoidanceQuality]",
        category: "Unit", id: "7ad9b9cfec5e37dbe3146382fcb03d1e")]
    public partial class SetUnitAvoidanceAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Unit;
        [SerializeReference] public BlackboardVariable<int> AvoidanceQuality;

        protected override Status OnStart()
        {
            if (!Unit.Value.TryGetComponent(out NavMeshAgent agent) || AvoidanceQuality > 4 || AvoidanceQuality < 0)
            {
                return Status.Failure;
            }
            
            agent.obstacleAvoidanceType = (ObstacleAvoidanceType) AvoidanceQuality.Value;
            return Status.Success;
        }
    }
}
