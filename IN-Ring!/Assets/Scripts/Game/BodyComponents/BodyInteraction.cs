using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

        [Space(30)]
        [Header("Required Components:")]
        [Space(5)]
        [SerializeField] private Body _mainBody;
        [SerializeField] private BodyPart[] _parts;
        [SerializeField] private ConfigurableJoint[] _joints;

        private int _maxHP;

        public bool IsDead { get; private set; }

        [Button]
        private void SetRequiredComponents()
        {
            _mainBody = GetComponent<Body>();
            _parts = GetComponentsInChildren<BodyPart>();
            _joints = GetComponentsInChildren<ConfigurableJoint>();
        }

        private void Start()
        {
            _maxHP = _HP;
        }

        public void ApplyDamage(int damage)
        {
            if (damage < 0)
            {
                throw new ArgumentException("The damage must be positive");
            }

            ProcessDamage(damage);

            _onHPChanged?.Invoke(_HP, _maxHP);
        }

        private void Die()
        {
            SwitchOffParts(_parts, _joints);

            IsDead = true;
            _mainBody.InvokeOuterOnDeadEvent();
            _onDead?.Invoke();
        }

        private void ProcessDamage(int damage)
        {
            _HP -= damage;

            if (_HP <= 0)
            {
                _HP = 0;
                Die();
            }
        }

        private void SwitchOffParts(IEnumerable<BodyPart> parts, IEnumerable<ConfigurableJoint> joints)
        {
            foreach (var part in parts)
            {
                part.enabled = false;
            }

            foreach (var joint in joints)
            {
                DeactivateJoint(joint);
            }
        }

        private void DeactivateJoint(ConfigurableJoint joint)
        {
            joint.connectedBody = null;
            joint.breakForce = 0;
            joint.breakTorque = 0;
            joint.xDrive = new JointDrive();
            joint.yDrive = new JointDrive();
            joint.zDrive = new JointDrive();
        }
    }
}