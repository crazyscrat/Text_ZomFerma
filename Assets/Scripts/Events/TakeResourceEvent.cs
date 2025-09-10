using Oko.EventBus;
using Oko.Reses;
using UnityEngine;

namespace Oko.vents
{
    public class TakeResourceEvent : IEvent
    {
        public EResources ResType { get; private set; }
        public int Amount { get; private set; }

        public TakeResourceEvent(EResources resType, int amount)
        {
            ResType = resType;
            Amount = amount;
        }
    }
}