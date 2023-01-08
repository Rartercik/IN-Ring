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
        [SerializeField] private float _animationSpeed;
        [SerializeField] private float _additionalVelocity;
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
            _animator.SetFloat(_speedParameter, _animationSpeed);
        }

        public void Move(Vector3 direction, bool worldSpace = false)
        {
            if (direction == Vector3.zero) return;

            direction = direction.normalized;
            direction = GetNearestPoint(direction, _animationBlendingPoints);
            SetAnimation(direction);
            SetVelocity(_rigidbody, _bodyParts, direction, true, _additionalVelocity);

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
            SetVelocity(_rigidbody, _bodyParts, Vector3.zero, false);

            _stopped = true;
        }

        private void SetAnimation(Vector3 movementDirection)
        {
            _animator.SetFloat(_movementX, movementDirection.x);
            _animator.SetFloat(_movementY, movementDirection.z);

            var swapping = Mathf.Abs(movementDirection.z) > _swapCoefficient;
            _animator.SetBool(_swapSide, swapping);

            _animator.SetBool(_legsMoving, true);
        }

        private void SetVelocity(Rigidbody root, IEnumerable<Rigidbody> bodyParts, Vector3 direction, bool isAdditional, float additionalVelocity = 0)
        {
            var velocityVector = direction * Time.fixedDeltaTime * additionalVelocity;

            var rootVelocity = isAdditional ? root.velocity : Vector3.zero;
            root.velocity = rootVelocity + root.rotation * velocityVector;

            foreach (var part in bodyParts)
            {
                var partVelocity = isAdditional ? part.velocity : Vector3.zero;
                part.velocity = partVelocity + root.rotation * velocityVector;
            }
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

        private JointDrive CreateDefaultJointDrive(float positionSpring)
        {
            var slerpDrive = new JointDrive();
            slerpDrive.maximumForce = float.MaxValue;
            slerpDrive.positionSpring = positionSpring;

            return slerpDrive;
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
