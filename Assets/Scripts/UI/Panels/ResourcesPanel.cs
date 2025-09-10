using System;
using System.Linq;
using Oko.Events;
using Oko.EventBus;
using UnityEngine;

namespace Oko.UI
{
    public class ResourcesPanel : MonoBehaviour
    {
        [SerializeField] private UIResourceItem[] _items;

        private void Awake()
        {
            if (_items == null || _items.Length == 0)
            {
                _items = GetComponentsInChildren<UIResourceItem>();
            }
        }

        private void Start()
        {
            Bus<UpdateResourceAmountEvent>.onEvent += HandlerUpdateResourceAmount;
        }

        private void OnDestroy()
        {
            Bus<UpdateResourceAmountEvent>.onEvent -= HandlerUpdateResourceAmount;
        }

        private void HandlerUpdateResourceAmount(UpdateResourceAmountEvent evt)
        {
            if(_items == null || _items.Length == 0) return;

            UIResourceItem resourceItem = _items.FirstOrDefault(i => i.ResType == evt.ResType);
            resourceItem?.UpdateResourceAmount(evt.Amount);
        }

        // public void UpdateAllItems()
        // {
        //     if(_items == null || _items.Length == 0) return;
        //     
        //     foreach (UIResourceItem item in _items)
        //     {
        //         item.UpdateResourceAmount();
        //     }
        // }
    }
}