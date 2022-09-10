using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.BodyComponents
{
    [RequireComponent(typeof(ConfigurableJoint))]
    public class BodyPart : MonoBehaviour
    {
        [SerializeField] private Transform _animationCopy;

        [Space(30)]
        [Header("Required Components:")]
        [Space(5)]
        [SerializeField] private ConfigurableJoint _joint;

        private Quaternion _startRotation;

        [Button]
        private void SetRequiredComponents()
        {
            _joint = GetComponent<ConfigurableJoint>();
        }

        private void Start()
        {
            _startRotation = transform.localRotation;
        }

        private void FixedUpdate()
        {
            _joint.targetRotation = Quaternion.Inverse(_animationCopy.localRotation) * _startRotation;
        }
    }
}