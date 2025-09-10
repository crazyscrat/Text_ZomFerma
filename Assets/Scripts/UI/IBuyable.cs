using Oko.Shop;

namespace Oko.UI
{
    public interface IBuyable
    {
        public CostPair[] Costs { get; }
    }
}