using System.Collections;
using System.Collections.Generic;
using Oko.Events;
using Oko.EventBus;
using Oko.Reses;
using Oko.UI;
using Oko.vents;
using UnityEngine;

namespace Oko.States
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [field: SerializeField] private EGameState State { get; set; }
        
        [SerializeField] private EUIState _uiState = EUIState.None; //debug serialize
        [SerializeField] private EGameState state => State; //debug serialize
        private PlayerResources _playerRes = new PlayerResources();
        
        #region Unity Methods

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;

                Init();
            }
        }

        private void Init()
        {
        }

        private void Start()
        {
            Bus<UnloadResourceEvent>.onEvent += HandlerUnloadResource;
            Bus<TakeResourceEvent>.onEvent += HandlerTakeResource;
            Bus<ChangeUIStateEvent>.onEvent += HandlerChangeUIState;
            Bus<ChangeGameStateEvent>.onEvent += HandlerChangeGameState;

            StartCoroutine(LateInit());
        }

        private void OnDestroy()
        {
            Bus<UnloadResourceEvent>.onEvent -= HandlerUnloadResource;
            Bus<TakeResourceEvent>.onEvent -= HandlerTakeResource;
            Bus<ChangeUIStateEvent>.onEvent -= HandlerChangeUIState;
            Bus<ChangeGameStateEvent>.onEvent -= HandlerChangeGameState;
        }

        #endregion

        #region Handlers

        private void HandlerChangeUIState(ChangeUIStateEvent evt)
        {
            _uiState = evt.State;
        }

        private void HandlerChangeGameState(ChangeGameStateEvent evt)
        {
            if(State == evt.State) return;
            
            State = evt.State;
            if (State == EGameState.Build || State == EGameState.Placement)
            {
                Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.Placement));
            }
        }

        private void HandlerUnloadResource(UnloadResourceEvent evt)
        {
            if(evt.Amount <= 0) return;
            _playerRes.Reses[evt.ResType] += evt.Amount;
            
            Bus<UpdateResourceAmountEvent>.Raise(new UpdateResourceAmountEvent(evt.ResType, _playerRes.Reses[evt.ResType]));
        }

        private void HandlerTakeResource(TakeResourceEvent evt)
        {
            if(_playerRes.Reses[evt.ResType] - evt.Amount >= 0)
            {
                _playerRes.Reses[evt.ResType] -= evt.Amount;
                Bus<UpdateResourceAmountEvent>.Raise(new UpdateResourceAmountEvent(evt.ResType, _playerRes.Reses[evt.ResType]));
            }
        }

        #endregion

        private IEnumerator LateInit()
        {
            yield return new WaitForSeconds(0.1f);
            
            Bus<ChangeUIStateEvent>.Raise(new ChangeUIStateEvent(EUIState.HUD));
            yield return null;
            
            foreach (KeyValuePair<EResources, int> resPair in _playerRes.Reses)
            {
                Bus<UpdateResourceAmountEvent>.Raise(new UpdateResourceAmountEvent(resPair.Key, resPair.Value));
            }
        }

        public int GetResourceAmount(EResources resource)
        {
            return _playerRes.Reses[resource];
        }
    }
}