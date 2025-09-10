using DG.Tweening;
using Oko.EventBus;
using Oko.Events;
using Oko.States;
using UnityEngine;

namespace Oko.UI
{
    public class QuickBuildingPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private ButtonQuickBuilding[] _quickButtons;

        private void Awake()
        {
            foreach (ButtonQuickBuilding quickButton in _quickButtons)
            {
                UQuickButtonType type = quickButton.QuickType;
                quickButton.Button.onClick.AddListener(()=>HandlerSelectQuick(type));
            }
        }

        private void HandlerSelectQuick(UQuickButtonType type)
        {
            Debug.Log(type.ToString());

            switch (type)
            {
                case UQuickButtonType.StopWork:
                    Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.HUD));
                    break;
                // case UQuickButtonType.SelectWork:
                //     Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.HUD));
                //     break;
                case UQuickButtonType.Move:
                    Bus<ChangeGameStateEvent>.Raise(new ChangeGameStateEvent(EGameState.Placement));
                    Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.HUD));
                    break;
                case UQuickButtonType.Sell:
                    Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.SellBuilding));
                    break;
                case UQuickButtonType.Upgrade:
                    Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.Upgrade));
                    break;
            }
            
        }

        public void SetVisibleAnim(bool visible)
        { _rectTransform.DOAnchorPosY(visible? 10 : -100, .3f);
        }
    }

    public enum UQuickButtonType
    {
        StopWork,
        SelectWork,
        Move,
        Sell,
        Upgrade,
    }
}