using Oko.EventBus;
using UnityEngine;

namespace Oko.Events
{
    public class HitPositionEvent : IEvent
    {
        public Vector3 Position { get; private set; }

        public HitPositionEvent(Vector3 position)
        {
            Position = position;
        }
    }
}