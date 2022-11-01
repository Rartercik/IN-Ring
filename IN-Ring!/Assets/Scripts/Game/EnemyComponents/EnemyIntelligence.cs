using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Game.EnemyComponents.States;
using Game.BodyComponents;
using Game.Tools;

namespace Game.EnemyComponents
{
    public class EnemyIntelligence : MonoBehaviour
    {
        [SerializeField] private Body _body;
        [SerializeField] private Body _player;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip[] _attacks;
        [SerializeField] private AnimationClip _winClip;
        [SerializeField] private float _maximalPlayerDistance;
        [SerializeField] private float _maximalPlayerAngle;
        [SerializeField] private float _rotationDelta;

        [Space(30)]
        [Header("Required Components:")]
        [Space(5)]
        [SerializeField] private Transform _transform;

        private EnemyState[] _states;

        public EnemyState State { get; private set; }
        public Body Body => _body;
        public Transform PlayerTransform => _playerTransform;

        [Button]
        private void SetRequiredComponents()
        {
            _transform = _body.transform;
        }

        private void OnValidate()
        {
            if (_animator.Contains(_winClip) == false)
            {
                throw new ArgumentException("Animator should contain your win animation");
            }

            foreach (var attack in _attacks)
            {
                if (_animator.Contains(attack) == false)
                {
                    throw new ArgumentException("Animator should contain your attack animation");
                }
            }
        }

        private void Start()
        {
            _player.OnDead += CelebrateVictory;

            _states = CreateAllStates();
        }

        private void FixedUpdate()
        {
            State.FixedUpdateLogic();
        }

        public void SwitchState<T>() where T : EnemyState
        {
            var state = _states.First(s => s is T);

            State = state;
        }

        public bool CheckDistanceCorrectness(Vector3 vectorToPlayer)
        {
            return vectorToPlayer.magnitude <= _maximalPlayerDistance;
        }

        public bool CheckRotationCorrectness(Vector3 vectorToTarget)
        {
            var rotationToPlayer = Vector3.Angle(_transform.forward, vectorToTarget);
            return rotationToPlayer <= _maximalPlayerAngle;
        }

        public bool IsAttacking()
        {
            foreach (var attack in _attacks)
            {
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName(attack.name))
                {
                    return true;
                }
            }

            return false;
        }

        private EnemyState[] CreateAllStates()
        {
            var states = new List<EnemyState>();
            var defaultState = new PursuesPlayer(this, _rotationDelta);
            State = defaultState;

            states.Add(defaultState);
            states.Add(new FightsPlayer(this));

            return states.ToArray();
        }

        private void CelebrateVictory()
        {
            _animator.Play(_winClip.name);
            enabled = false;
        }
    }
}