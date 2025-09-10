using Oko.EventBus;
using UnityEngine;

namespace Oko.Events
{
    public class MoveToResourceEvent : IEvent
    {
        public GameObject Target { get; private set; }

        public MoveToResourceEvent(GameObject resource)
        {
            Target = resource;
        }
    }
}