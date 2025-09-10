using UnityEngine;
using UnityEngine.AI;

namespace Oko.Units
{
    public interface IMovable
    {
        public NavMeshAgent Agent { get; }

        public void MoveToPoint(Vector3 position);
        public void MoveToStation(Vector3 position);
        public void MoveToWorkPlace(Vector3 position);
        public void Stop();
    }
}