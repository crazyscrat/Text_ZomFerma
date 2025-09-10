using UnityEngine.AI;

namespace Oko.Units
{
    public interface IPlaced
    {
        public NavMeshObstacle NavObstacle { get; }
    }
}