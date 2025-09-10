using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Oko.UI
{
    public class ButtonQuickResource : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _name;
        [SerializeField] private EQuickResourceAction _action;
        
        private void Awake()
        {
            _text.SetText(_name);
        }

        public Button Button => _button;

        public EQuickResourceAction ResourceAction => _action;
    }
}