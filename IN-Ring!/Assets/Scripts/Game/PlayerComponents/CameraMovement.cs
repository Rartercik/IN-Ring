using UnityEngine;

namespace Game.PlayerComponents
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        private Transform _transform;
        private Vector3 _offset;

        private void Start()
        {
            _transform = transform;
            _offset = _transform.position - _target.position;
        }

        private void LateUpdate()
        {
            var target = _target.position + _target.rotation *  _offset;
            _transform.position = target;

            var targetRotation = Quaternion.Euler(_transform.eulerAngles.x, _target.eulerAngles.y, _transform.eulerAngles.z);
            _transform.rotation = targetRotation;
        }
    }
}
