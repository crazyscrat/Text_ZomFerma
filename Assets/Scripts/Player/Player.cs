using Oko.Events;
using Oko.EventBus;
using Oko.Reses;
using Oko.Units;
using UnityEngine;

namespace Oko.Player
{
    public class Player : AUnit
    {
        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            
            Bus<HitPositionEvent>.onEvent += HandlerGridPointSelected;
            Bus<MoveToResourceEvent>.onEvent += HandlerMoveToResource;
        }

        private void OnDestroy()
        {
            Bus<HitPositionEvent>.onEvent -= HandlerGridPointSelected;
            Bus<MoveToResourceEvent>.onEvent -= HandlerMoveToResource;
        }

        #endregion

        private void HandlerGridPointSelected(HitPositionEvent evt)
        {
            MoveToPoint(evt.Position);
        }

        private void HandlerMoveToResource(MoveToResourceEvent evt)
        {
            GraphAgent.SetVariableValue<GameObject>("TargetObject", null);
            GraphAgent.SetVariableValue("Resource", evt.Target.GetComponent<ARes>());
            GraphAgent.SetVariableValue("Command", ECommand.MoveToResource);
        }
    }
}