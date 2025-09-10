using Oko.Reses;
using UnityEngine;

namespace Oko.Shop
{
    [CreateAssetMenu(fileName = "Resource Shop Item SO", menuName = "Shop/Resource Shop Item SO", order = 1)]
    public class ResourceShopItemSO : AShopItemSO
    {
        [field: SerializeField] public AResourceSO ResourceSo { get; private set; }
    }
}