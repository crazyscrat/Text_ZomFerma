using System;
using UnityEngine;

namespace Oko.Shop
{
    public abstract class AShopItemSO : ScriptableObject
    {
        [field: SerializeField] public EShopItemType Type { get; private set; }
        [field: SerializeField] public CostPair[] Costs { get; private set; }
    }

    [Serializable]
    public enum EShopItemType
    {
        Station,
        Resource,
    }
}