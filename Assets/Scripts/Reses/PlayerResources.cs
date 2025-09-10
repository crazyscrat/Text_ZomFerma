using System;
using System.Collections.Generic;

namespace Oko.Reses
{
    [Serializable]
    public class PlayerResources
    {
        public Dictionary<EResources, int> Reses { get; set; } = new Dictionary<EResources, int>()
        {
            {EResources.Alc, 10},
            {EResources.Conalc, 0},
            {EResources.Winalc, 0},
            {EResources.Gold, 10000},
            {EResources.Wood, 0},
            {EResources.Stone, 0},
            {EResources.Iron, 0},
            {EResources.Silicon, 0},
            {EResources.Marble, 0},
            {EResources.ChipSimple, 0},
            {EResources.ChipPerfected, 0},
            {EResources.AIChipSimple, 3},
            {EResources.AIChipPerfected, 0},
        };
    }
}