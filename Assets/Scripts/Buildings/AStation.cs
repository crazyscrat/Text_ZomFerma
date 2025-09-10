using System;
using Oko.Events;
using Oko.EventBus;
using Oko.Reses;
using Oko.Units;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Oko.Buildings
{
    public class AStation : MonoBehaviour, IStation, IPlaced
    {
        #region Fields

        [field: SerializeField] public StationSO StationSo { get; private set; }
        [field: SerializeField] public AResourceSO ResSO { get; private set; }
        public NavMeshObstacle NavObstacle { get; private set; }
        [SerializeField] protected ScriptableObject WorkerSO;
        [SerializeField] private WorkerPlace[] workerPlaces;
        public Transform Transform { get; private set; }
        public ARes Target { get; private set; }
        public bool CanPlaced { get; private set; }
        
        private WorkerUnit[] _workers;
        private Rigidbody _rigidbody;


        //test
        [SerializeField] private GameObject _prefabWorker;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Transform = transform;
            _workers = new WorkerUnit[workerPlaces.Length];
            if(TryGetComponent(out NavMeshObstacle navObstacle))
            {
                NavObstacle = navObstacle;
                NavObstacle.enabled = false;
            }

            _rigidbody = transform.AddComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }

        private void Start()
        {
            //SpawnWorkers();
            
            //Bus<BuildStationEvent>.Raise(new BuildStationEvent(this));
        }

        private void OnTriggerEnter(Collider other)
        {
            CanPlaced = false;
            Debug.Log(CanPlaced);
        }

        private void OnTriggerExit(Collider other)
        {
            CanPlaced = true;
            Debug.Log(CanPlaced);
        }

        #endregion

        public void StartPlacement()
        {
            NavObstacle.enabled = false;
            
            _rigidbody = transform.AddComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
            
            Target = null;
            
            foreach (WorkerPlace workerPlace in workerPlaces)
            {
                workerPlace.Worker.gameObject.SetActive(false);
                workerPlace.Worker.gameObject.transform.SetPositionAndRotation(workerPlace.Place.transform.position, workerPlace.Place.transform.rotation);
                workerPlace.Worker.Reset();
            }
        }
        
        public void PlacementFinished()
        {
            Destroy(_rigidbody);
            
            foreach (WorkerPlace workerPlace in workerPlaces)
            {
                workerPlace.Worker.gameObject.SetActive(true);
            }
            
            if(NavObstacle != null)
            {
                NavObstacle.enabled = true;
            }
        }
        
        public void BuildFinished()
        {
            Destroy(_rigidbody);
            SpawnWorkers();
            if(NavObstacle != null)
            {
                NavObstacle.enabled = true;
            }
        }
        
        public void SpawnWorkers()
        {
            for (int i = 0; i < workerPlaces.Length; i++)
            {
                //create worker
                GameObject worker = Instantiate(_prefabWorker, workerPlaces[i].Place.position, Quaternion.identity);
                worker.transform.SetParent(gameObject.transform);
                _workers[i] = worker.GetComponent<WorkerUnit>();
                
                _workers[i].SetStation(this);
                workerPlaces[i].SetWorker(_workers[i]);
            }
            
            //test
            // ARes res = FindObjectsByType<ARes>(FindObjectsSortMode.None).First(r => r.ResSo.ResType == ResSO.ResType);
            // SetTarget(res);
        }

        public void SetTarget(ARes target)
        {
            //set resource target
            Target = target;
            foreach (WorkerUnit worker in _workers)
            {
                worker.SetResource(Target);
                worker.MoveToResource();
            }
        }

        public void AddMountResources(int amount)
        {
            Bus<UnloadResourceEvent>.Raise(new UnloadResourceEvent(ResSO.ResType, amount));
            
            //Debug.Log(GameManager.Instance.PlayerRes.Reses[EResources.Wood]);
        }

        public Transform GetPlace(WorkerUnit worker)
        {
            for (int i = 0; i < workerPlaces.Length; i++)
            {
                if(workerPlaces[i].Worker.Equals(worker))
                {
                    return workerPlaces[i].Place;
                }
            }
            return null;
        }

    }

    [Serializable]
    public struct WorkerPlace
    {
        [field: SerializeField] public Transform Place { get; set; }
        [field: SerializeField] public WorkerUnit Worker { get; private set; }
        public bool IsEmpty => Worker == null;

        public void SetWorker(WorkerUnit worker)
        {
            Worker = worker;
        }
    }
}