using Oko.EventBus;
using UnityEngine;

namespace Oko.Events
{
    public class GridPointSelectedEvent : IEvent
    {
        public Vector3 Point { get; private set; }

        public GridPointSelectedEvent(Vector3 point)
        {
            Point = point;
        }
    }
}