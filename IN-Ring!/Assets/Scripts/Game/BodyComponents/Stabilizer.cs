using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Rigidbody))]
public class Stabilizer : MonoBehaviour
{
    [SerializeField] private float _minSpring;
    [SerializeField] private float _maxSpring;
    [SerializeField] private float _maxAngleDeviation;

    [Space(30)]
    [Header("Required Components:")]
    [Space(5)]
    [SerializeField] private ConfigurableJoint _joint;
    [SerializeField] private Rigidbody _rigidbody;

    private Quaternion _startAngle;

    [Button]
    private void SetRequiredComponents()
    {
        _joint = GetComponent<ConfigurableJoint>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _startAngle = _rigidbody.rotation;
    }

    private void FixedUpdate()
    {
        var deviation = GetDeviation();
        var deviationProgress = deviation / _maxAngleDeviation;

        SetSlerpDrive(deviationProgress);
    }

    private void SetSlerpDrive(float deviationProgress)
    {
        var spring = Mathf.Lerp(_minSpring, _maxSpring, deviationProgress);
        var slerpDrive = CreateDefaultSlerpDrive(spring);

        _joint.slerpDrive = slerpDrive;
    }

    private JointDrive CreateDefaultSlerpDrive(float spring)
    {
        var slerpDrive = new JointDrive();
        slerpDrive.positionSpring = spring;
        slerpDrive.positionDamper = _joint.slerpDrive.positionDamper;
        slerpDrive.maximumForce = _joint.slerpDrive.maximumForce;

        return slerpDrive;
    }

    private float GetDeviation()
    {
        var currentAngle = _rigidbody.rotation;
        var targetAngle = _joint.targetRotation * _startAngle;

        return Quaternion.Angle(targetAngle, currentAngle);
    }
}
