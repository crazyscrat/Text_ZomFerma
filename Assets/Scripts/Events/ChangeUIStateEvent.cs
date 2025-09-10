using Oko.EventBus;
using Oko.UI;
using UnityEngine;

namespace Oko.Events
{
    public class ChangeUIStateEvent : IEvent
    {
        public EUIState State { get; private set; }
        public GameObject SelectedGameObject{ get; private set; }

        public ChangeUIStateEvent(EUIState state, GameObject selectedGameObject = null)
        {
            State = state;
            SelectedGameObject = selectedGameObject;
        }
    }
}