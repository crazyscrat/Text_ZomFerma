using Oko.EventBus;
using Oko.States;

namespace Oko.Events
{
    public class ChangeGameStateEvent : IEvent
    {
        public EGameState State { get; private set; }

        public ChangeGameStateEvent(EGameState state)
        {
            State = state;
        }
    }
}