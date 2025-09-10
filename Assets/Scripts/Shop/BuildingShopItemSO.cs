using Oko.Buildings;
using UnityEngine;

namespace Oko.Shop
{
    [CreateAssetMenu(fileName = "Building Shop Item SO", menuName = "Shop/Building Shop Item SO", order = 1)]
    public class BuildingShopItemSO : AShopItemSO
    {
        [field: SerializeField] public StationSO StationSo { get; private set; }
    }
}