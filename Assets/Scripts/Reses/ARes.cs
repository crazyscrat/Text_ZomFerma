using System;
using System.Collections.Generic;
using System.Linq;
using Oko.Buildings;
using Oko.Utilites;
using UnityEngine;

namespace Oko.Reses
{
    public abstract class ARes : MonoBehaviour
    {
        [field: SerializeField] public AResourceSO ResSo { get; private set; }
        [field: SerializeField] public int RemainAmount { get; private set; }
        [field: SerializeField] public int AmountAtOnce { get; private set; }

        private void Start()
        {
            if (ResSo != null)
            {
                RemainAmount = ResSo.Amount;
                AmountAtOnce = ResSo.AmountAtOnce;
            }
        }

        public int TakeResource()
        {
            if(RemainAmount > AmountAtOnce)
            {
                RemainAmount -= AmountAtOnce;
                return AmountAtOnce;
            }
            else
            {
                int amount = RemainAmount;
                RemainAmount = 0;
                return amount;
            }
        }

        public void CheckAmountAvailable()
        {
            if (RemainAmount == 0)
            {
                try
                {
                    Destroy(gameObject);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }

        public int PlayerTakeResource(int takeAmount)
        {
            if(RemainAmount > AmountAtOnce)
            {
                RemainAmount -= AmountAtOnce;
                return AmountAtOnce;
            }
            else
            {
                int amount = RemainAmount;
                RemainAmount = 0;
                return amount;
            }
        }

        public void FindStationToWork()
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            AStation[] stations = FindObjectsByType<AStation>(FindObjectsSortMode.None);
            List<AStation> stationsList = stations.Where(s => s.ResSO.ResType == ResSo.ResType && s.Target == null).ToList();
            
            if(stationsList.Count > 0)
            {
                stationsList.Sort(new ClosestFreeStation(transform.position));
                stationsList[0].SetTarget(this);
            }
        }
    }
}