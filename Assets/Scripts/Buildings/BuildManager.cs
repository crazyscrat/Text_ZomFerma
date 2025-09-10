using Oko.Events;
using Oko.EventBus;
using Oko.Shop;
using Oko.States;
using Oko.vents;
using UnityEngine;

namespace Oko.Buildings
{
    public class BuildManager : MonoBehaviour
    {
        private GameObject _buildingInstance;
        private AShopItemSO _buildingItemSO;

        #region Unity Methods

        private void Start()
        {
            Bus<GridPointSelectedEvent>.onEvent += HandlerGridPointSelected;
            Bus<BuildStationEvent>.onEvent += HandlerBuildStation;
            Bus<BuildStationFinishedEvent>.onEvent += HandlerBuildStationFinished;
            Bus<CheckPlacedBuildingEvent>.onEvent += HandlerCheckPlacedBuilding;
            Bus<PlacementStartingEvent>.onEvent += HandlerPlacementStarting;
        }

        private void OnDestroy()
        {
            Bus<GridPointSelectedEvent>.onEvent -= HandlerGridPointSelected;
            Bus<BuildStationEvent>.onEvent -= HandlerBuildStation;
            Bus<BuildStationFinishedEvent>.onEvent -= HandlerBuildStationFinished;
            Bus<CheckPlacedBuildingEvent>.onEvent -= HandlerCheckPlacedBuilding;
            Bus<PlacementStartingEvent>.onEvent -= HandlerPlacementStarting;
        }

        #endregion

        #region Handlers

        private void HandlerGridPointSelected(GridPointSelectedEvent evt)
        {
            if (_buildingInstance == null) return;

            _buildingInstance.transform.position = evt.Point;
        }

        private void HandlerBuildStation(BuildStationEvent evt)
        {
            _buildingItemSO = evt.ItemSo;
            BuildingShopItemSO buildingSo = _buildingItemSO as BuildingShopItemSO;
            CreateBuilding(buildingSo.StationSo.Prefab);
        }

        private void HandlerCheckPlacedBuilding(CheckPlacedBuildingEvent evt)
        {
            if(_buildingInstance == null) return;
            if (_buildingInstance.TryGetComponent(out AStation station))
            {
                if (station.CanPlaced)
                {
                    if(_buildingItemSO != null)
                    {
                        //build
                        FinishBuild();
                    }
                    else
                    {
                        //placement
                        PlacementFinished();
                    }
                    
                    Bus<ChangeGameStateEvent>.Raise(new ChangeGameStateEvent(EGameState.Base));
                }
                else
                {
                    //todo message error placement building
                }
            }
        }

        private void HandlerPlacementStarting(PlacementStartingEvent evt)
        {
            _buildingInstance = evt.Building;
            AStation station = _buildingInstance.GetComponent<AStation>();
            station.StartPlacement();
            
            Bus<ChangeGameStateEvent>.Raise(new ChangeGameStateEvent(EGameState.Placement));
        }

        private void HandlerBuildStationFinished(BuildStationFinishedEvent evt)
        {
            FinishBuild();
        }

        #endregion

        private void CreateBuilding(GameObject prefab)
        {
            _buildingInstance = Instantiate(prefab);
            Bus<ChangeGameStateEvent>.Raise(new ChangeGameStateEvent(EGameState.Build));
        }

        private void PlacementFinished()
        {
            AStation station = _buildingInstance.GetComponent<AStation>();
            station.PlacementFinished();
            _buildingInstance = null;
        }

        private void FinishBuild()
        {
            foreach (CostPair costPair in _buildingItemSO.Costs)
            {
                //remove cost resources
                Bus<TakeResourceEvent>.Raise(new TakeResourceEvent(costPair.resource, costPair.amount));
            }

            AStation station = _buildingInstance.GetComponent<AStation>();
            station.BuildFinished();
            
            _buildingInstance = null;
            _buildingItemSO = null;
        }
    }
}