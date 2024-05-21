using System;
using UnityEngine;

namespace CameraFollowSpace
{
    public class CameraFollow: MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothSpeed = 0.9f;
        [SerializeField] private Transform _endLevelSideTransform; 

        private Vector3 _startPosition;
        private float widthCamera;
        
        private void Start()
        {
            _startPosition = transform.position;
            
            Vector3 leftSidePosition = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
            Vector3 rightSidePosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0));

            widthCamera = rightSidePosition.x - leftSidePosition.x;
        }

        private void LateUpdate()
        {
            
            Vector3 desirePosition = new Vector3(_targetTransform.position.x, _startPosition.y, _startPosition.z) + _offset;
            Vector3 smoothPosition = Vector3.Lerp(_startPosition, desirePosition, _smoothSpeed);

            if (smoothPosition.x + (widthCamera / 2) > _endLevelSideTransform.position.x)
            {
                smoothPosition = new Vector3(_endLevelSideTransform.position.x - (widthCamera / 2), _startPosition.y, _startPosition.z);
            } else if (smoothPosition.x < 0)
            {
                smoothPosition = _startPosition;
            }
            
            transform.position = smoothPosition;
        }
    }
}