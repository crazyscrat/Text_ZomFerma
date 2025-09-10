using System;
using Oko.EventBus;
using Oko.Events;
using UnityEngine;

namespace Oko.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private ResourcesPanel _resourcesPanel;
        [SerializeField] private BuildingsShopPanel _buildingsShopPanel;
        [SerializeField] private QuickBuildingPanel _quickBuildingPanel;
        [SerializeField] private QuickResourcePanel _quickResourcePanel;

        [SerializeField] private EUIState _uiState = EUIState.None;
        
        private Camera _camera;
        private GameObject _selectedGameObject;

        private void Awake()
        {
            _camera = Camera.main;
            HandlerChangeUIState(new ChangeUIStateEvent(EUIState.None));
        }

        private void Start()
        {
            Bus<ChangeUIStateEvent>.onEvent += HandlerChangeUIState;
        }

        private void OnDestroy()
        {
            Bus<ChangeUIStateEvent>.onEvent -= HandlerChangeUIState;
        }

        private void HandlerChangeUIState(ChangeUIStateEvent evt)
        {
            switch (evt.State)
            {
                case EUIState.None:
                    _resourcesPanel.gameObject.SetActive(false);
                    _buildingsShopPanel.gameObject.SetActive(false);
                    _quickBuildingPanel.gameObject.SetActive(false);
                    _quickResourcePanel.gameObject.SetActive(false);
                    
                    _uiState = evt.State;
                    break;
                case EUIState.HUD:
                    _resourcesPanel.gameObject.SetActive(true);
                    //_quickBuildingPanel.gameObject.SetActive(false);
                    _quickBuildingPanel.SetVisibleAnim(false);
                    _buildingsShopPanel.gameObject.SetActive(false);
                    _quickResourcePanel.gameObject.SetActive(false);
                    
                    _uiState = evt.State;
                    break;
                case EUIState.BuildingsShop:
                    if (_buildingsShopPanel.gameObject.activeInHierarchy)
                    {
                        //_uiState = evt.State;
                        _buildingsShopPanel.gameObject.SetActive(false);
                        Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.HUD));
                    }
                    else
                    {
                        _uiState = evt.State;
                        _buildingsShopPanel.gameObject.SetActive(true);
                    }
                    break;
                case EUIState.QuickMenuBuilding:
                    if (_uiState == EUIState.QuickMenuBuilding)
                    {
                        _selectedGameObject = null;
                        Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.HUD));
                    }
                    else
                    {
                        //_uiState = EUIState.QuickMenu;
                        _uiState = evt.State;
                        _selectedGameObject = evt.SelectedGameObject;
                        _quickBuildingPanel.gameObject.SetActive(true);
                        _quickBuildingPanel.SetVisibleAnim(true);
                    }
                    break;
                case EUIState.QuickMenuResource:
                    if (_uiState == EUIState.QuickMenuResource)
                    {
                        _selectedGameObject = null;
                        Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.HUD));
                    }
                    else
                    {
                        //_uiState = EUIState.QuickMenu;
                        _uiState = evt.State;
                        _selectedGameObject = evt.SelectedGameObject;
                        _quickResourcePanel.ShowPanel(_selectedGameObject);
                        _quickResourcePanel.gameObject.SetActive(true);
                    }
                    break;
                case EUIState.MainMenu:
                case EUIState.PauseMenu:
                case EUIState.SettingsMenu:
                    _uiState = evt.State;
                    break;
                case EUIState.Placement:
                    _uiState = evt.State;
                    if(_selectedGameObject != null)
                    {
                        //for replacement, not build
                        Bus<PlacementStartingEvent>.Raise(new PlacementStartingEvent(_selectedGameObject));
                    }
                    _buildingsShopPanel.gameObject.SetActive(false);
                    break;
                default:
                    Debug.LogError($"Not found {evt.State}");
                    break;
            }
        }
    }
}