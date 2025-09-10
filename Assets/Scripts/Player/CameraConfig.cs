using System;
using UnityEngine;

namespace Oko.Player
{
    [Serializable]
    public class CameraConfig
    {
        [field: SerializeField] public float KeyboardPanSpeedHorizontal { get; set; } = 15f;
        [field: SerializeField] public float KeyboardPanSpeedVertical { get; set; } = 20f;
        [field: SerializeField] public float KeyboardPanSpeedSprint { get; set; } = 2f;
        [field: SerializeField] public float RotationSpeed { get; set; } = 1f;
        [field: SerializeField] public float ZoomSpeed { get; set; } = 1f;
        [field: SerializeField] public float DefaultZoomDistance { get; set; } = 20f;
        [field: SerializeField] public float MinZoomDistance { get; set; } = 15f;
        [field: SerializeField] public float MaxZoomDistance { get; set; } = 25f;
    }
}