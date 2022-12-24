using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.BodyComponents
{
    public class BodyMovement : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ConfigurableJoint _mainJoint;
        [SerializeField] private GameObject _bodyPartsParent;
        [SerializeField] private float _slerpPositionSpring;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private Foot[] _feet;

        [Space(30)]
        [Header("Required Components:")]
        [Space(5)]
        [SerializeField] private Rigidbody[] _bodyParts;

        private readonly string _legsMoving = "Moving";
        private readonly string _speedParameter = "MovementSpeed";
        private readonly string _swapSide = "SwapSide";
        private readonly float _swapCoefficient = 0.8f;
        private readonly string _movementX = "MovementX";
        private readonly string _movementY = "MovementY";
        private readonly Vector3[] _animationBlendingPoints = new Vector3[]
        {
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1),
            new Vector3(-1, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(-0.7f, 0, -0.7f),
            new Vector3(0.7f, 0, -0.7f),
            new Vector3(-0.7f, 0, 0.7f),
            new Vector3(0.7f, 0, 0.7f)
        };

        private Quaternion _startRotation;
        private bool _stopped;

        [Button]
        private void SetRequiredComponents()
        {
            _bodyParts = _bodyPartsParent.GetComponentsInChildren<Rigidbody>();
        }

        private void Start()
        {
            _startRotation = transform.localRotation;
            _animator.SetFloat(_speedParameter, _movementSpeed);
        }

        public void Move(Vector3 direction, bool worldSpace = false)
        {
            if (direction == Vector3.zero) return;

            direction = direction.normalized;
            direction = GetNearestPoint(direction, _animationBlendingPoints);

            _animator.SetFloat(_movementX, direction.x);
            _animator.SetFloat(_movementY, direction.z);

            var swapping = Mathf.Abs(direction.z) > _swapCoefficient;
            _animator.SetBool(_swapSide, swapping);

            foreach (var foot in _feet)
            {
                foot.SetZeroFriction();
            }

            _animator.SetBool(_legsMoving, true);
            _stopped = false;
        }

        public void Rotate(float yRotation)
        {
            _mainJoint.slerpDrive = CreateDefaultJointDrive(_slerpPositionSpring);

            var finalRotationY = _rigidbody.rotation.eulerAngles.y + (yRotation * _rotationSpeed);
            var finalRotation = _mainJoint.targetRotation;
            var finalEulerRotation = finalRotation.eulerAngles;
            finalEulerRotation.y = finalRotationY;
            finalRotation.eulerAngles = finalEulerRotation;

            _mainJoint.targetRotation = Quaternion.Inverse(finalRotation) * _startRotation;
        }

        public void Jump()
        {
            if (CheckJumpAvailable())
            {
                foreach (var part in _bodyParts)
                {
                    part.AddForce(Vector3.up * _jumpForce, ForceMode.Acceleration);
                }
            }
        }

        public void Stop()
        {
            if (_stopped) return;

            _animator.SetBool(_legsMoving, false);
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);

            foreach (var part in _bodyParts)
            {
                part.velocity = Vector3.zero;
            }

            _stopped = true;
        }

        public void StopFeet()
        {
            foreach (var foot in _feet)
            {
                foot.SetMaxFriction();
            }
        }

        private JointDrive CreateDefaultJointDrive(float positionSpring)
        {
            var slerpDrive = new JointDrive();
            slerpDrive.maximumForce = float.MaxValue;
            slerpDrive.positionSpring = positionSpring;

            return slerpDrive;
        }

        private bool CheckJumpAvailable()
        {
            foreach (var foot in _feet)
            {
                if (foot.OnFloor == false)
                {
                    return false;
                }
            }

            return true;
        }

        private Vector3 GetNearestPoint(Vector3 target, IEnumerable<Vector3> points)
        {
            var nearest = points.First();

            foreach(var point in points)
            {
                var vectorToPoint = point - target;
                var vectorToNearest = nearest - target;

                if (vectorToPoint.magnitude < vectorToNearest.magnitude)
                {
                    nearest = point;
                }
            }

            return nearest;
        }
    }
}
