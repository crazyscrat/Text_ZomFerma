using Oko.Events;
using Oko.EventBus;
using Oko.Reses;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Oko.UI
{
    public class QuickResourcePanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private ButtonQuickResource[] _quickButtons;
        [SerializeField] private Vector2 _offset = new Vector2(0, 10);
        
        private GameObject _resourceSelected;

        private void Awake()
        {
            foreach (ButtonQuickResource quickButton in _quickButtons)
            {
                EQuickResourceAction resourceAction = quickButton.ResourceAction;
                quickButton.Button.onClick.AddListener(()=>HandlerSelectQuick(resourceAction));
            }
        }

        private void HandlerSelectQuick(EQuickResourceAction resourceAction)
        {
            //select resource
            Debug.Log(resourceAction.ToString());
            switch (resourceAction)
            {
                case EQuickResourceAction.HandleWork:
                    //player work on resource
                    Bus<MoveToResourceEvent>.Raise(new MoveToResourceEvent(_resourceSelected));
                    break;
                case EQuickResourceAction.StationWork:
                    //select nearest station and set work
                    _resourceSelected.GetComponent<ARes>().FindStationToWork();
                    break;
            }
            
            Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.QuickMenuResource));
            _resourceSelected = null;
        }

        public void ShowPanel(GameObject resource)
        {
            _resourceSelected = resource;
            SetPositionOnScreen();
        }

        private void SetPositionOnScreen()
        {
            _rectTransform.position = Mouse.current.position.ReadValue() + _offset;
        }
    }

    public enum EQuickResourceAction
    {
        HandleWork,
        StationWork,
    }
}
