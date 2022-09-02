using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Rigidbody))]
public class Stabilizer : MonoBehaviour
{
    [SerializeField] private float _minSpring;
    [SerializeField] private float _maxSpring;
    [SerializeField] private float _maxAngleDeviation;

    private ConfigurableJoint _joint;
    private Rigidbody _rigidbody;
    private Quaternion _startAngle;

    private void Start()
    {
        _joint = GetComponent<ConfigurableJoint>();
        _rigidbody = GetComponent<Rigidbody>();
        _startAngle = _rigidbody.rotation;
    }

    private void FixedUpdate()
    {
        var deviation = GetDeviation();
        var deviationProgress = deviation / _maxAngleDeviation;

        var spring = Mathf.Lerp(_minSpring, _maxSpring, deviationProgress);
        var slerpDrive = new JointDrive();
        slerpDrive.positionSpring = spring;
        slerpDrive.positionDamper = _joint.slerpDrive.positionDamper;
        slerpDrive.maximumForce = _joint.slerpDrive.maximumForce;

        _joint.slerpDrive = slerpDrive;
    }

    private float GetDeviation()
    {
        var currentAngle = _rigidbody.rotation;
        var targetAngle = _joint.targetRotation * _startAngle;

        return Quaternion.Angle(targetAngle, currentAngle);
    }
}
