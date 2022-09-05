using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.BodyComponents
{
    [RequireComponent(typeof(Body))]
    public class BodyInteraction : MonoBehaviour
    {
        [SerializeField] private int _HP;
        [SerializeField] private UnityEvent<int, int> _onHPChanged;
        [SerializeField] private UnityEvent _onDead;

        private Body _mainBody;
        private BodyPart[] _parts;
        private ConfigurableJoint[] _joints;
        private int _maxHP;

        public bool IsDead { get; private set; }

        private void Start()
        {
            _mainBody = GetComponent<Body>();
            _parts = GetComponentsInChildren<BodyPart>();
            _joints = GetComponentsInChildren<ConfigurableJoint>();
            _maxHP = _HP;
        }

        public void ApplyDamage(int damage)
        {
            if (damage < 0)
            {
                throw new ArgumentException("The damage must be positive");
            }

            _HP -= damage;
            if (_HP <= 0)
            {
                _HP = 0;
                Die();
            }

            _onHPChanged?.Invoke(_HP, _maxHP);
        }

        private void Die()
        {
            foreach (var part in _parts)
            {
                part.enabled = false;
            }

            foreach (var joint in _joints)
            {
                joint.connectedBody = null;
                joint.breakForce = 0;
                joint.breakTorque = 0;
                joint.xDrive = new JointDrive();
                joint.yDrive = new JointDrive();
                joint.zDrive = new JointDrive();
            }

            IsDead = true;
            _mainBody.InvokeOuterOnDeadEvent();
            _onDead?.Invoke();
        }
    }
}