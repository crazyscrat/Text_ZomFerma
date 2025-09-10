using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Oko.UI
{
    public class ButtonQuickBuilding : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private UQuickButtonType _quickType;
        [SerializeField] private string _name;

        private void Awake()
        {
            _text.SetText(_name);
        }

        public Button Button => _button;
        public UQuickButtonType QuickType => _quickType;
    }
}