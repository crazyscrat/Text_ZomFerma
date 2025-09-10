using Oko.EventBus;
using UnityEngine;

namespace Oko.Events
{
    public class PlacementStartingEvent : IEvent
    {
        public GameObject Building { get; private set; }

        public PlacementStartingEvent(GameObject building)
        {
            Building = building;
        }
    }
}