using Oko.Buildings;
using Oko.Reses;

namespace Oko.Units
{
    public interface IWorker
    {
        public IStation CurrentStation { get; }
        public ARes Resource { get; }

        public void SetResource(ARes resource);
        public void SetStation(IStation station);
        public void MoveToResource();
        public void WorkAtResource();
        public void MoveToStation();
        public void WorkAtStation();
        public void Unloading();
        public void Reset();
    }
}