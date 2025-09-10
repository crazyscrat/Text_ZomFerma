using Oko.Events;
using Oko.EventBus;
using UnityEngine;

namespace Oko.Grid
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private GridConfig _gridConfig;

        private float _step;
        private float _halfStep;
        
        #region Unity Methods

        private void Awake()
        {
            _step = _gridConfig.GridStep;
            _halfStep = _gridConfig.GridStep * .5f;
        }

        private void Start()
        {
            Bus<RaycastPointEvent>.onEvent += HandlerRaycastPoint;
        }

        private void OnDestroy()
        {
            Bus<RaycastPointEvent>.onEvent -= HandlerRaycastPoint;
        }

        #endregion

        #region Events

        private void HandlerRaycastPoint(RaycastPointEvent evt)
        {
            //X coord rounded for Grid Step
            float rayX = evt.Point.x;
            int multiplier = rayX >= 0 ? 1 : -1;
            int stepX = (int)(rayX / _step); 
            float posX = stepX * _step;
            float overStep = rayX % _step;
            posX += Mathf.Abs(overStep) > _halfStep ? _step * multiplier : 0f;
            
            //Z coord rounded for Grid Step
            float rayZ = evt.Point.z;
            multiplier = rayZ >= 0 ? 1 : -1;
            int stepZ = (int)(rayZ / _step); 
            float posZ = stepZ * _step;
            overStep = rayZ % _step;
            posZ += Mathf.Abs(overStep) > _halfStep ? _step * multiplier : 0f;
            
            Vector3 gridPosition = new Vector3(posX, 0f, posZ);
            //Debug.Log(gridPosition);
            Bus<GridPointSelectedEvent>.Raise(new GridPointSelectedEvent(gridPosition));
        }

        #endregion
    }
}