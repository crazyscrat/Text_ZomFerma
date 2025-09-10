using System;
using Oko.Buildings;
using Oko.Units;
using Oko.Utilites;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Oko.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "UnloadResources", story: "[Unit] unload [Amount] resources to [Station]", category: "Unit",
        id: "38691e5930e98a8d41d576c0d0b060bc")]
    public partial class UnloadResourcesAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Unit;
        [SerializeReference] public BlackboardVariable<int> Amount;
        [SerializeReference] public BlackboardVariable<GameObject> Station;
        [SerializeReference] public BlackboardVariable<ECommand> Command;

        private float _endTime;
        private Animator _animator;
        
        protected override Status OnStart()
        {
            //todo station SO time unloading
            _endTime = Time.time + 6f;
            _animator = Unit.Value.GetComponent<AUnit>().Animator;
            _animator?.SetBool(AnimationConstants.WORK, true);
            
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (Time.time < _endTime)
            {
                return Status.Running;
            }
            return Status.Success;
        }

        protected override void OnEnd()
        {
            Station.Value.GetComponent<AStation>().AddMountResources(Amount.Value);
            Amount.Value = 0;
            _animator?.SetBool(AnimationConstants.WORK, false);

            if (Station.Value.GetComponent<AStation>().Target == null)
            {
                Command.Value = ECommand.Free;
            }
            else
            {
                Command.Value = ECommand.MoveToResource;
            }
        }
    }
}
