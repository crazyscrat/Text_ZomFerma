using UnityEngine;

namespace Oko.Reses
{
    public abstract class AResourceSO : ScriptableObject
    {
        [field: SerializeField] public EResources ResType { get; private set; }
        [field: SerializeField] public string ResName { get; private set; }
        [field: SerializeField] public Sprite ResIcon { get; private set; }
        [field: SerializeField] public float WorkTime { get; private set; } = 3f;
        [field: SerializeField] public int Amount { get; private set; } = 50;
        [field: SerializeField] public int AmountAtOnce { get; private set; } = 2;
    }
}