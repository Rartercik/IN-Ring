using UnityEngine;

namespace Game.BodyComponents
{
    public class ChestFlexioner : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _maxDeviation;

        private float _xRotation;

        public float XRotation => _xRotation;

        private void OnAnimatorIK(int layerIndex)
        {
            var chest = _animator.GetBoneTransform(HumanBodyBones.Chest);
            var chestRotation = chest.rotation;
            chestRotation = Quaternion.Euler(_xRotation, chestRotation.eulerAngles.y, chestRotation.eulerAngles.z);

            _animator.SetBoneLocalRotation(HumanBodyBones.Chest, chestRotation);
        }

        public void RotateXDimension(float xRotation)
        {
            var rotation = _xRotation + xRotation;
            var finalRotation = Mathf.Clamp(rotation, -_maxDeviation, _maxDeviation);

            _xRotation = finalRotation;
        }
    }
}
