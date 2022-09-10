using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Game.Tools;

namespace Game.BodyComponents
{
    [RequireComponent(typeof(Collider))]
    public class HitDetector : MonoBehaviour
    {
        [SerializeField] private int _hitDamage;
        [SerializeField] private float _hitForce;
        [SerializeField] private Animator _hitAnimator;
        [SerializeField] private AnimationClip _hitAnimation;
        [SerializeField] private LayerMask _enemy;

        [Space(30)]
        [Header("Required Components:")]
        [Space(5)]
        [SerializeField] private Collider _collider;

        private bool _canAttack = true;

        [Button]
        private void SetRequiredComponents()
        {
            _collider = GetComponent<Collider>();
        }

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
                    Hit(enemy, other);
                }
            }
        }

        public void SetAttackAvailable()
        {
            _canAttack = true;
        }

        private void Hit(DamageApplier enemy, Collider enemyPart)
        {
            enemy.ApplyDamage(_hitDamage);
            AddForceTo(enemy, enemyPart);
            _canAttack = false;
        }

        private void AddForceTo(DamageApplier enemy, Collider enemyPart)
        {
            var detectorPosition = _collider.bounds.center;
            var hitPosition = enemyPart.ClosestPointOnBounds(detectorPosition);
            var directionToHit = (hitPosition - detectorPosition).normalized;
            var hitForce = directionToHit * _hitForce;

            enemy.Rigidbody.AddForceAtPosition(hitForce, hitPosition, ForceMode.Acceleration);
        }
    }
}