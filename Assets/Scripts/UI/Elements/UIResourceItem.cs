using System;
using Oko.Reses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Oko.UI

{
    public class UIResourceItem : MonoBehaviour
    {
        [field: SerializeField] public EResources ResType { get; private set; }
        [field: SerializeField] public Sprite SpriteIcon { get; private set; }
        [Space]
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _amount;

        private const string FORMAT = "{0}: {1}";

        private void Awake()
        {
            if(SpriteIcon != null && _icon != null)
            {
                _icon.sprite = SpriteIcon;
            }
        }

        public void UpdateResourceAmount(EResources resType, int amount)
        {
            if (resType != ResType) return;

            _amount.SetText(string.Format(FORMAT, resType.ToString(), amount));
        }

        public void UpdateResourceAmount(int amount)
        {
            _amount.SetText(string.Format(FORMAT, ResType.ToString(), amount));
        }
    }
}