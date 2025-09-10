using Oko.EventBus;
using Oko.Events;
using Oko.Shop;
using UnityEngine;
using UnityEngine.UI;

namespace Oko.UI
{
    public class ButtonShopItem : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private AShopItemSO _shopItemSo;
        [SerializeField] private CostPanel _costPanel;

        private void Awake()
        {
            _button.onClick.AddListener(HandlerOnClick);
        }

        private void Start()
        {
            if (_shopItemSo != null)
            {
                _costPanel?.SetCost(_shopItemSo.Costs);
            }
        }

        private void HandlerOnClick()
        {
            switch (_shopItemSo.Type)
            {
                case EShopItemType.Station:
                    //BuildingShopItemSO buildingSo = _shopItemSo as BuildingShopItemSO;
                    Bus<BuildStationEvent>.Raise(new BuildStationEvent(_shopItemSo));
                    break;
                case EShopItemType.Resource:
                    ResourceShopItemSO resourceSo = _shopItemSo as ResourceShopItemSO;
                    
                    break;
                default:
                    Debug.LogError($"Not found {_shopItemSo.Type}");
                    break;
            }
        }
    }
}