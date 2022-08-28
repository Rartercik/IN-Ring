using System;
using UnityEngine;
using Game.Tools;

namespace Game.BodyComponents
{
    public class HitDetector : MonoBehaviour
    {
        [SerializeField] private int _hitDamage;
        [SerializeField] private Animator _hitAnimator;
        [SerializeField] private AnimationClip _hitAnimation;
        [SerializeField] private LayerMask _enemy;

        private bool _canAttack = true;

        private void OnValidate()
        {
            if (gameObject.scene.name == null || _hitAnimator.Contains(_hitAnimation)) return;

            throw new ArgumentException("Animator should contain your animation");
        }

        private void OnTriggerEnter(Collider other)
        {
            var currentState = _hitAnimator.GetCurrentAnimatorStateInfo(0);

            if (currentState.IsName(_hitAnimation.name) && LayerTool.EqualLayers(_enemy, other.gameObject.layer) && _canAttack)
            {
                if (other.TryGetComponent(out DamageApplier enemy))
                {
                    enemy.ApplyDamage(_hitDamage);
                    _canAttack = false;
                }
            }
        }

        public void SetAttackAvailable()
        {
            _canAttack = true;
        }
    }
}