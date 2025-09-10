using Oko.Units.SO;
using UnityEngine;

namespace Oko.Units
{
    public interface IUnit
    {
        public Transform Transform { get; }
        public Animator Animator { get; }
        public AUnitSO UnitSo { get; }
    }
}