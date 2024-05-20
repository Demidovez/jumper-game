using UnityEngine;

namespace CameraFollowSpace
{
    public class CameraFollow: MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothSpeed = 0.125f;

        private void LateUpdate()
        {
            Vector3 desirePosition = _targetTransform.position + _offset;
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desirePosition, _smoothSpeed);

            transform.position = smoothPosition;
        }
    }
}