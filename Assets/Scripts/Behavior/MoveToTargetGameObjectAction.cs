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
    [NodeDescription(name: "Move To Target GameObject", story: "[Unit] move to [TargetObject] ",
        category: "Action/Navigation", id: "32083f3e75ba7d1f01d8530d9a8e2684")]
    public partial class MoveToTargetGameObjectAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Unit;
        [SerializeReference] public BlackboardVariable<GameObject> TargetObject;
        [SerializeReference] public BlackboardVariable<float> MoveThreshold = new BlackboardVariable<float>(0.25f);

        private NavMeshAgent _agent;
        private Animator _animator;
        private Vector3 _lastPosition;
        
        protected override Status OnStart()
        {
            AUnit movable = Unit.Value.GetComponent<AUnit>();
            _agent = movable.Agent;
            _animator = movable.Animator;

            if (_animator != null)
            {
                _animator.SetBool(AnimationConstants.REST, false);
            }
            
            Vector3 targetPosition = GetTargetPosition();
            if (Vector3.Distance(targetPosition, _lastPosition) >= MoveThreshold)
            {
                _agent.SetDestination(targetPosition);
                _lastPosition = _agent.destination;
                return Status.Running;
            }
            
            if(!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                return Status.Success;
            }
            
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (_animator != null)
            {
                _animator.SetFloat(AnimationConstants.SPEED, _agent.velocity.magnitude);
            }

            Vector3 targetPosition = GetTargetPosition();
            if (Vector3.Distance(targetPosition, _lastPosition) >= MoveThreshold)
            {
                _agent.SetDestination(targetPosition);
                _lastPosition = _agent.destination;
                return Status.Running;
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

        private Vector3 GetTargetPosition()
        {
            Vector3 targetPosition;
            if (TargetObject.Value.TryGetComponent(out Collider collider))
            {
                targetPosition = collider.ClosestPoint(_agent.transform.position);
            }
            else
            {
                targetPosition = TargetObject.Value.transform.position;
            }
            
            return targetPosition;
        }
    }
}
