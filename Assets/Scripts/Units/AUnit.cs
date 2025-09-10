using Oko.Units.SO;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace Oko.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AUnit : MonoBehaviour, IUnit, IMovable
    {
        public Transform Transform { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public AUnitSO UnitSo { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        protected BehaviorGraphAgent GraphAgent { get; private set; }

        #region Unity Methods

        protected virtual void Awake()
        {
            Transform = transform;
            
            if (TryGetComponent(out BehaviorGraphAgent graphAgent))
            {
                GraphAgent = graphAgent;
            }
            
            Agent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Start()
        {
            
        }

        #endregion

        #region Movement Methods

        public virtual void MoveToPoint(Vector3 position)
        {
            GraphAgent.SetVariableValue<GameObject>("TargetObject", null);
            GraphAgent.SetVariableValue("TargetPoint", position);
            GraphAgent.SetVariableValue("Command", ECommand.MoveToPosition);
        }

        public virtual void MoveToStation(Vector3 position) { }

        public virtual void MoveToWorkPlace(Vector3 position) { }

        public virtual void Stop()
        {
            GraphAgent.SetVariableValue("Command", ECommand.Stop);
        }

        #endregion
    }
}