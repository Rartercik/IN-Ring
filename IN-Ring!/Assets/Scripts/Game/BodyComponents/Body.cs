using System;
using UnityEngine;

namespace Game.BodyComponents
{
    public class Body : MonoBehaviour
    {
        [SerializeField] private BodyMovement _movement;
        [SerializeField] private BodyInteraction _interaction;
        [SerializeField] private Animator _legsAnimator;
        [SerializeField] private Animator _spineAnimator;

        public event Action OnDead;

        private readonly string _legsMoving = "Moving";
        private readonly string _spinePunch = "Punch";

        public bool IsDead => _interaction.IsDead;

        public void Move(Vector3 _direction, bool worldSpace = false)
        {
            _movement.Move(_direction, worldSpace);
            _legsAnimator.SetBool(_legsMoving, true);
        }

        public void Rotate(float yRotation)
        {
            _movement.Rotate(yRotation);
        }

        public void TryJump()
        {
            _movement.Jump();
        }

        public void Stop()
        {
            _movement.Stop();
            _legsAnimator.SetBool(_legsMoving, false);
        }

        public void ApplyDamage(int damage)
        {
            if (IsDead == false)
            {
                _interaction.ApplyDamage(damage);
            }
        }

        public void Punch()
        {
            _spineAnimator.SetTrigger(_spinePunch);
        }

        public void InvokeOuterOnDeadEvent()
        {
            OnDead?.Invoke();
        }
    }
}
