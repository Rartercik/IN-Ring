using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.PlayerComponents
{
    public class CameraCenter : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private ConfigurableJoint _playerMainJoint;
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationSpeed;

        [Space(30)]
        [Header("Required Components:")]
        [Space(5)]
        [SerializeField] private Transform _transform;

        [Button]
        private void SetRequiredComponents()
        {
            _transform = transform;
        }

        private void Update()
        {
            if (_playerMainJoint == null) return;

            var targetPosition = _player.position;
            MoveTo(targetPosition);

            var targetRotation = Quaternion.Inverse(_playerMainJoint.targetRotation);
            RotateTo(targetRotation);
        }

        private void MoveTo(Vector3 target)
        {
            var step = _speed * Time.deltaTime;
            _transform.position = Vector3.MoveTowards(_transform.position, target, step);
        }

        private void RotateTo(Quaternion target)
        {
            var rotationStep = _rotationSpeed * Time.deltaTime;
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, target, rotationStep);
        }
    }
}
