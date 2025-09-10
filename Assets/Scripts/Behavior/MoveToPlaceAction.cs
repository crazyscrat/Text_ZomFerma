using System;
using Oko.Units;
using Oko.Utilites;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Oko.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "MoveToPlace", story: "[Unit] move to [Place] and Rest", category: "Unit",
        id: "b4186edc3afc2312a4ddc2550e298331")]
    public partial class MoveToPlaceAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Unit;
        [SerializeReference] public BlackboardVariable<Vector3> Place;

        protected override Status OnStart()
        {
            WorkerUnit workerUnit = Unit.Value.GetComponent<WorkerUnit>();
            //Place.Value = workerUnit.CurrentStation.GetPlace(workerUnit).position;
            Unit.Value.transform.position = workerUnit.CurrentStation.GetPlace(workerUnit).position;
            Unit.Value.transform.rotation = workerUnit.CurrentStation.GetPlace(workerUnit).rotation;
            
            workerUnit.Animator.SetBool(AnimationConstants.REST, true);
            
            return Status.Success;
        }
    }
}
