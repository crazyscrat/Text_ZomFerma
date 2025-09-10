using System;
using UnityEngine;

namespace Oko.Grid
{
    [Serializable]
    public class GridConfig
    {
        [field: SerializeField] public int GridStep { get; set; } = 1;
        [field: SerializeField] public int GridWidth { get; set; } = 100;
        [field: SerializeField] public int GridHeight { get; set; } = 100;
    }
}