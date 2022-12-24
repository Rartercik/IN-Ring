using System;
using UnityEngine;

namespace Game.BodyComponents
{
    public class Body : MonoBehaviour
    {
        [SerializeField] private BodyMovement _movement;
        [SerializeField] private ChestFlexioner _chestFlexioner;
        [SerializeField] private BodyInteraction _interaction;
        [SerializeField] private Animator _animator;

        public event Action OnDead;

        private readonly string _spinePunch = "Punch";

        public bool IsDead => _interaction.IsDead;

        public void Move(Vector3 _direction, bool worldSpace = false)
        {
            _movement.Move(_direction, worldSpace);
        }

        public void Rotate(float yRotation)
        {
            _movement.Rotate(yRotation);
        }

        public void RotateChestXDimension(float xRotation)
        {
            _chestFlexioner.RotateXDimension(xRotation);
        }

        public void TryJump()
        {
            _movement.Jump();
        }

        public void Stop()
        {
            _movement.Stop();
        }

        public void ApplyDamage(int damage)
        {
            if (IsDead == false)
            {
                _interaction.ApplyDamage(damage);
            }
        }

        public void MakeImmortal()
        {
            _interaction.MakeImmortal();
        }

        public void Punch()
        {
            _animator.SetTrigger(_spinePunch);
        }

        public void InvokeOuterOnDeadEvent()
        {
            OnDead?.Invoke();
        }
    }
}
