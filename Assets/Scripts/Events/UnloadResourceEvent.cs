using Oko.EventBus;
using Oko.Reses;

namespace Oko.Events
{
    public class UnloadResourceEvent : IEvent
    {
        public EResources ResType { get; private set; }
        public int Amount { get; private set; }

        public UnloadResourceEvent(EResources resType, int amount)
        {
            ResType = resType;
            Amount = amount;
        }
    }
}