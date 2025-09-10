using Oko.EventBus;
using Oko.Reses;

namespace Oko.Events
{
    public class UpdateResourceAmountEvent : IEvent
    {
        public EResources ResType { get; private set; }
        public int Amount { get; private set; }

        public UpdateResourceAmountEvent(EResources resType, int amount)
        {
            ResType = resType;
            Amount = amount;
        }
    }
}