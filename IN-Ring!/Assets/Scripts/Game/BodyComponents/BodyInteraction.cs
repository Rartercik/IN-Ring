using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using RootMotion.Dynamics;

namespace Game.BodyComponents
{
    [RequireComponent(typeof(Body))]
    public class BodyInteraction : MonoBehaviour
    {
        [SerializeField] private PuppetMaster _puppetMaster;
        [SerializeField] private ConfigurableJoint _mainJoint;
        [SerializeField] private int _HP;
        [SerializeField] private UnityEvent<int, int> _onHPChanged;
        [SerializeField] private UnityEvent _onDead;

        [Space(30)]
        [Header("Required Components:")]
        [Space(5)]
        [SerializeField] private Body _mainBody;

        private int _maxHP;

        public bool IsDead { get; private set; }

        [Button]
        private void SetRequiredComponents()
        {
            _mainBody = GetComponent<Body>();
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
            DeactivateMuscles(_puppetMaster);

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

        private void DeactivateMuscles(PuppetMaster puppetMaster)
        {
            var musclesCount = puppetMaster.muscles.Length;

            for (int i = 0; i < musclesCount; i++)
            {
                _puppetMaster.DisconnectMuscleRecursive(0, MuscleDisconnectMode.Explode);
            }
        }
    }
}