using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.PlayerComponents
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        [Space(30)]
        [Header("Required Components:")]
        [Space(5)]
        [SerializeField] private Transform _transform;

        private Vector3 _offset;

        [Button]
        private void SetRequiredComponents()
        {
            _transform = transform;
        }

        private void Start()
        {
            _offset = _transform.position - _target.position;
        }

        private void LateUpdate()
        {
            SetTargetPosition();

            SetTargetRotation();
        }

        private void SetTargetPosition()
        {
            var target = _target.position + _target.rotation * _offset;
            _transform.position = target;
        }

        private void SetTargetRotation()
        {
            var targetRotation = Quaternion.Euler(_transform.eulerAngles.x, _target.eulerAngles.y, _transform.eulerAngles.z);
            _transform.rotation = targetRotation;
        }
    }
}
