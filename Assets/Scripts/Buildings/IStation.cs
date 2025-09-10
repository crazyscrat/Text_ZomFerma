using Oko.Reses;
using Oko.Units;
using UnityEngine;

namespace Oko.Buildings
{
    public interface IStation : IBuilding
    {
        public StationSO StationSo { get; }
        public ARes Target {get; }
        public void SpawnWorkers();
        public void SetTarget(ARes target);
        public void AddMountResources(int amount);
        public Transform GetPlace(WorkerUnit worker);
    }
}