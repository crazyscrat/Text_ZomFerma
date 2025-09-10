using Unity.Behavior;

namespace Oko.Units
{
    [BlackboardEnum]
    public enum ECommand
    {
        Idle,
        Free,
        Stop,
        MoveToPosition,
        MoveToResource,
        MoveToStation,
        Work,
        Unloading,
    }
}