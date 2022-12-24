using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.BodyComponents;
using Game.FightingEnvironment.GameStates;

namespace Game.FightingEnvironment
{
    public class GameStatesHandler : MonoBehaviour
    {
        [SerializeField] private Body _player;
        [SerializeField] private Body _enemy;
        [SerializeField] private Canvas _victory;
        [SerializeField] private Canvas _defeat;
        [SerializeField] private float _timeBeforeResult;

        private GameState[] _states;

        public GameState State { get; private set; }

        private void Start()
        {
            _states = CreateAllStates(out var defaultState);

            State = defaultState;
        }

        public void ReportPlayerVictory()
        {
            State.ReportPlayerVictory();
        }

        public void ReportPlayerDeafeat()
        {
            State.ReportPlayerDeafeat();
        }

        public void SwitchState<T>() where T : GameState
        {
            var state = _states.First(s => s is T);
            state.Enter();

            State = state;
        }

        private GameState[] CreateAllStates(out GameState defaultState)
        {
            var states = new List<GameState>();
            defaultState = new Fighting(this);

            states.Add(defaultState);
            states.Add(new PlayerVictory(this, _player, () => StartCoroutine(ShowResultAfter(_victory, _timeBeforeResult))));
            states.Add(new PlayerDefeat(this, _enemy, () => StartCoroutine(ShowResultAfter(_defeat, _timeBeforeResult))));

            return states.ToArray();
        }

        private IEnumerator ShowResultAfter(Canvas result, float seconds)
        {
            yield return new WaitForSeconds(seconds);

            result.enabled = true;
        }
    }
}
