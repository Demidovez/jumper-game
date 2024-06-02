using System;
using GameManagementSpace;
using UnityEngine;

namespace CameraFollowSpace
{
    public class CameraFollow: MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothSpeed = 0.9f;

        private Vector3 _startPosition;
        private Camera _mainCamera;
        private float _xMin, _xMax, _yMin, _yMax;
        private float _camX, _camY;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            var height = _mainCamera.orthographicSize;
            var width = height * _mainCamera.aspect;

            var bounds = Global.Instance.LevelBounds;
            
            _xMin = bounds.min.x + width;
            _xMax = bounds.max.x - width;
            _yMin = bounds.min.y + height;
            _yMax = bounds.max.y - height;
            
            _startPosition = new Vector3(_xMin, _yMin, transform.position.z);
            transform.position = _startPosition;
        }

        private void LateUpdate()
        {
            _camX = Mathf.Clamp(_targetTransform.position.x, _xMin, _xMax);
            _camY = Mathf.Clamp(_targetTransform.position.y, _yMin , _yMax );
            
            Vector3 desirePosition = new Vector3(_camX, _camY, _startPosition.z) + _offset;
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desirePosition, _smoothSpeed);

            transform.position = smoothPosition;
        }
    }
}