using System.Collections.Generic;
using Oko.Buildings;
using UnityEngine;

namespace Oko.Utilites
{
    public class ClosestFreeStation : IComparer<AStation>
    {
        private Vector3 _targetPosition;
        private AStation _station;

        public ClosestFreeStation(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
        }

        public int Compare(AStation x1, AStation x2)
        {
            float d1 = Vector3.Distance(_targetPosition, x1.Transform.position);
            float d2 = Vector3.Distance(_targetPosition, x2.Transform.position);
            return d1.CompareTo(d2);
        }
    }
}