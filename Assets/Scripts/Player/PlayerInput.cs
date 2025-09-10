using DG.Tweening;
using Oko.Events;
using Oko.Buildings;
using Oko.EventBus;
using Oko.Reses;
using Oko.States;
using Oko.UI;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Oko.Player
{
    public class PlayerInput : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private Rigidbody _cameraTarget;
        [SerializeField] private CameraConfig _cameraConfig;
        [SerializeField] private Vector3 _startingFollowOffset = new Vector3(0f, 20f, 20f);
        [SerializeField] private LayerMask _layerMaskFloor;
        [SerializeField] private LayerMask _layerMaskOverFloor;
        [SerializeField] private LayerMask _layerMaskFloorAndOver;

        #endregion

        #region Properties

        private EUIState _uiState = EUIState.None;
        private EGameState _gameState = EGameState.Base;
        private CinemachineFollow _cinemachineFollow;
        private float _maxRotationAmount;
        private Transform _transform;
        private float _sprintSpeed;
        private Vector3 _linearVelocity = Vector3.zero;
        private bool _isPointerOverUI;

        #endregion
        
        #region Unity Methods

        private void Awake()
        {
            _transform = transform;
            
            if (!_cinemachineCamera.TryGetComponent(out _cinemachineFollow))
            {
                Debug.LogError("No Cinemachine Follow found");
            }
            else
            {
                _cinemachineFollow.FollowOffset = _startingFollowOffset;
                _maxRotationAmount = Mathf.Abs(_cinemachineFollow.FollowOffset.z);
            }

            _cinemachineCamera.Lens.OrthographicSize = _cameraConfig.DefaultZoomDistance;
            _sprintSpeed = 1f;
        }

        private void Start()
        {
            Bus<ChangeUIStateEvent>.onEvent += HandlerChangeUIState;
            Bus<ChangeGameStateEvent>.onEvent += HandlerChangeGameState;
        }

        private void Update()
        {
            HandleMove();
            _isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
        }

        private void OnDestroy()
        {
            Bus<ChangeUIStateEvent>.onEvent -= HandlerChangeUIState;
            Bus<ChangeGameStateEvent>.onEvent -= HandlerChangeGameState;
        }

        #endregion

        #region Inputs

        //wasd
        public void Move(InputAction.CallbackContext context)
        {
            Vector2 moveAmount = context.ReadValue<Vector2>();
            _linearVelocity = _transform.TransformDirection(
                new Vector3(
                    moveAmount.x * _cameraConfig.KeyboardPanSpeedHorizontal, 
                    0, 
                    moveAmount.y * _cameraConfig.KeyboardPanSpeedVertical));
        }

        //shift
        public void Sprint(InputAction.CallbackContext context)
        {
            if (context.performed || context.canceled)
            {
                _sprintSpeed = context.ReadValueAsButton() ? _cameraConfig.KeyboardPanSpeedSprint : 1f;
            }
        }

        //left click
        public void MainAction(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (_isPointerOverUI) return;
            
            if (_uiState == EUIState.PauseMenu)
            {
                return;
            }
            
            if (_uiState == EUIState.QuickMenuBuilding)
            {
                //close quick menu
                Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.QuickMenuBuilding));
                return;
            }
            
            if (_uiState == EUIState.QuickMenuResource)
            {
                //close quick menu
                Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.QuickMenuResource));
                return;
            }
            
            if (_uiState == EUIState.BuildingsShop)
            {
                //close building shop
                Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.BuildingsShop));
                return;
            }
            
            if (_gameState == EGameState.Base)
            {
                RaycastHit hit = new RaycastHit();
                if (GetRaycastHit(ref hit, _layerMaskFloorAndOver))
                {
                    GameObject hitGameObject = hit.collider.gameObject;
                    int objectLayer = hitGameObject.layer;
                    
                    if(objectLayer == LayerMask.NameToLayer("Floor"))
                    {
                        //move player to position
                        Bus<HitPositionEvent>.Raise(new HitPositionEvent(hit.point));
                    }
                    else if (objectLayer == LayerMask.NameToLayer("Resource"))
                    {
                        //quick menu resource
                        Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.QuickMenuResource, hitGameObject));
                    }
                    else if (objectLayer == LayerMask.NameToLayer("Building"))
                    {
                        //quick menu building
                        Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.QuickMenuBuilding, hitGameObject));
                    }
                }
                // else if (!EventSystem.current.IsPointerOverGameObject()
                //          && Physics.Raycast(ray, out hit, float.MaxValue, interactableLayer | floorLayers))
                // {
                //     
                // }
            }
            else if (_gameState == EGameState.Build)
            {
                Bus<CheckPlacedBuildingEvent>.Raise(new CheckPlacedBuildingEvent());
            }
            else if (_gameState == EGameState.Placement)
            {
                Bus<CheckPlacedBuildingEvent>.Raise(new CheckPlacedBuildingEvent());
            }
        }

        //right click
        public void AdditionalAction(InputAction.CallbackContext context)
        {
            return;
            if (!context.performed) return;
            if (_isPointerOverUI) return;
            
            if (_uiState == EUIState.QuickMenuBuilding)
            {
                //open-close quick menu
                Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.QuickMenuBuilding));
                return;
            }
            
            if (_gameState != EGameState.Placement)
            {
                RaycastHit hit = new RaycastHit();
                if (GetRaycastHit(ref hit, _layerMaskOverFloor))
                {
                    if(hit.collider.TryGetComponent(out IBuilding building))
                    {
                        //building menu
                        Debug.LogWarning("building menu");
                        Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.QuickMenuBuilding));
                    }
                    else if(hit.collider.TryGetComponent(out ARes resource))
                    {
                        //resource menu
                        Debug.LogWarning("resource menu");
                        Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.QuickMenuBuilding));
                    }
                }
            }
        }

        //placement building or other construction on grid
        public void CursorPosition(InputAction.CallbackContext context)
        {
            if ((_gameState == EGameState.Placement
                 || _gameState == EGameState.Build) 
                && context.performed)
            {
                RaycastHit hit = new RaycastHit();
                if (GetRaycastHit(ref hit, _layerMaskFloor))
                {
                    //if(hit.collider.CompareTag("Floor"))
                    {
                        //move building to position
                        Bus<RaycastPointEvent>.Raise(new RaycastPointEvent(hit.point));
                    }
                }
            }
        }
        
        //zoom
        public void Zoom(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (_uiState != EUIState.HUD) return;
            
            Vector3 offset = _cinemachineFollow.FollowOffset;

            offset.y += context.ReadValue<float>() * _cameraConfig.ZoomSpeed;
            offset.z += context.ReadValue<float>() * _cameraConfig.ZoomSpeed;
            offset.y = Mathf.Clamp(offset.y, 
                _startingFollowOffset.y - _cameraConfig.MinZoomDistance, 
                _startingFollowOffset.y + _cameraConfig.MaxZoomDistance);
            offset.z = Mathf.Clamp(offset.z, 
                _startingFollowOffset.z - _cameraConfig.MinZoomDistance, 
                _startingFollowOffset.z + _cameraConfig.MaxZoomDistance);
            
            //_cinemachineFollow.FollowOffset =  offset;
            
            DOTween.To(()=>_cinemachineFollow.FollowOffset, x=> _cinemachineFollow.FollowOffset = x, offset, .2f);
            
        }
        
        //zoom reset
        public void ZoomReset(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                //reset rotation
                // Vector3 offset = _cinemachineFollow.FollowOffset;
                // offset.x = _cameraConfig.DefaultZoomDistance;
                // _cinemachineFollow.FollowOffset =  offset;
                Vector3 offset = _startingFollowOffset;
                _cinemachineFollow.FollowOffset =  offset;
            }
        }

        public void BuildingsShop(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            
            Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.BuildingsShop));
        }
        
        #endregion

        private void HandlerChangeUIState(ChangeUIStateEvent evt)
        {
            _uiState = evt.State;
        }
        
        private void HandlerChangeGameState(ChangeGameStateEvent evt)
        {
            _gameState = evt.State;
        }
        
        private void HandleMove()
        {
            _cameraTarget.linearVelocity = _linearVelocity * _sprintSpeed;
        }

        private bool GetRaycastHit(ref RaycastHit hit, LayerMask layerMask)
        {
            if(_mainCamera == null) return false;
            
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask))
            {
                return true;
            }
            
            return false;
        }
    }
}