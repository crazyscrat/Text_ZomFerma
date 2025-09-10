using System;
using Oko.Reses;

namespace Oko.Shop
{
    [Serializable]
    public struct CostPair
    {
        public EResources resource;
        public int amount;
    }
}