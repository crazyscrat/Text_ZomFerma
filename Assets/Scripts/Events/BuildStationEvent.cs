using Oko.EventBus;
using Oko.Shop;

namespace Oko.Events
{
    public class BuildStationEvent : IEvent
    {
        public AShopItemSO ItemSo { get; private set; }

        public BuildStationEvent(AShopItemSO itemSo)
        {
            ItemSo = itemSo;
        }
    }
}