using UnityEngine;

public class CameraCenter : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private ConfigurableJoint _playerMainJoint;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    private void Update()
    {
        var targetPosition = _player.position;
        var step = _speed * Time.deltaTime;
        _transform.position = Vector3.MoveTowards(_transform.position, targetPosition, step);

        var targetRotation = Quaternion.Inverse(_playerMainJoint.targetRotation);
        var rotationStep = _rotationSpeed * Time.deltaTime;
        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, targetRotation, step);
    }
}
