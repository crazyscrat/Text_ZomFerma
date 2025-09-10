using Oko.Buildings;
using Oko.Reses;

namespace Oko.Units
{
    public class WorkerUnit : AUnit, IWorker
    {
        public IStation CurrentStation { get; private set; }
        public ARes Resource { get; private set; }

        public void SetResource(ARes resource)
        {
            Resource = resource;
            GraphAgent.SetVariableValue("Resource", Resource);
        }

        public void SetStation(IStation station)
        {
            CurrentStation = station;
            GraphAgent.SetVariableValue("Station", station.Transform.gameObject);
        }

        public void MoveToResource()
        {
            if(Resource == null) return;
            GraphAgent.SetVariableValue("Command", ECommand.MoveToResource);
        }

        public void WorkAtResource()
        {
        }

        public void MoveToStation()
        {
            GraphAgent.SetVariableValue("Command", ECommand.MoveToStation);
        }

        public void WorkAtStation()
        {
        }

        public void Unloading()
        {
            
        }

        public void Reset()
        {
            Resource = null;
            GraphAgent.SetVariableValue<ARes>("Resource", null);
            GraphAgent.SetVariableValue("Command", ECommand.Stop);
        }
    }
}