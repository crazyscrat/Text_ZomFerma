using Oko.Reses;
using System;
using Oko.EventBus;
using Oko.Events;
using Oko.Units;
using Oko.Utilites;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Oko.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "PlayerWorkOnResource", story: "[Unit] work on [Resource] and take [Amount] res", category: "Unit", id: "aa37ff6c2c55e51cdb53ad0c1e66f5e3")]
    public partial class PlayerWorkOnResourceAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Unit;
        [SerializeReference] public BlackboardVariable<ARes> Resource;
        [SerializeReference] public BlackboardVariable<int> Amount;
            
        private float _endTime;
        private Animator _animator;
        
        protected override Status OnStart()
        {
            if(Resource.Value == null) return Status.Failure;
            _animator = Unit.Value.GetComponent<AUnit>().Animator;
            _animator?.SetBool(AnimationConstants.WORK_ON_RESOURCE, true);
            
            _endTime = Time.time + Resource.Value.ResSo.WorkTime;
            
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
            //todo amount resources take player for energy or tools
            Amount.Value = Resource.Value.PlayerTakeResource(10);
            
            _animator?.SetBool(AnimationConstants.WORK_ON_RESOURCE, false);
            Bus<UnloadResourceEvent>.Raise(new UnloadResourceEvent(Resource.Value.ResSo.ResType, Amount.Value));
            Resource.Value.CheckAmountAvailable();
        }
    }
}
