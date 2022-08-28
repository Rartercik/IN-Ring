using UnityEngine;

namespace Game.BodyComponents
{
    [RequireComponent(typeof(ConfigurableJoint))]
    public class BodyPart : MonoBehaviour
    {
        [SerializeField] private Transform _animationCopy;

        private ConfigurableJoint _joint;
        private Quaternion _startRotation;

        private void Start()
        {
            _joint = GetComponent<ConfigurableJoint>();
            _startRotation = transform.localRotation;
        }

        private void FixedUpdate()
        {
            _joint.targetRotation = Quaternion.Inverse(_animationCopy.localRotation) * _startRotation;
        }
    }
}