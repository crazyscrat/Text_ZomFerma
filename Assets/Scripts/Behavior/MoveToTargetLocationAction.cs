using System;
using Oko.Units;
using Oko.Utilites;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

namespace Oko.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Move to Target Location", story: "[Unit] move to [TargetPoint]",
        category: "Action/Navigation", id: "1c66161efd3b4126a0d44bddd3b88e86")]
    public partial class MoveToTargetLocationAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Unit;
        [SerializeReference] public BlackboardVariable<Vector3> TargetPoint;

        private NavMeshAgent _agent;
        private Animator _animator;
        
        protected override Status OnStart()
        {
            AUnit movable = Unit.Value.GetComponent<AUnit>();
            _agent = movable.Agent;
            _animator = movable.Animator;
            
            if (_animator != null)
            {
                _animator.SetBool(AnimationConstants.REST, false);
            }
            
            if (Vector3.Distance(_agent.transform.position, TargetPoint.Value) <= _agent.stoppingDistance)
            {
                return Status.Success;
            }
            
            _agent.SetDestination(TargetPoint.Value);
            
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (_animator != null)
            {
                _animator.SetFloat(AnimationConstants.SPEED, _agent.velocity.magnitude);
            }
            
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                return Status.Success;
            }
            
            return Status.Running;
        }

        protected override void OnEnd()
        {
            if (_animator != null)
            {
                _animator.SetFloat(AnimationConstants.SPEED, 0f);
            }
        }
    }
}
