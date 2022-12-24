using System;
using UnityEngine;
using Game.BodyComponents;

namespace Game.FightingEnvironment.GameStates
{
    public class PlayerDefeat : GameState
    {
        private readonly Body _enemy;
        private readonly Action _onEntered;

        public PlayerDefeat(GameStatesHandler statesHandler, Body enemy, Action onEntered) : base(statesHandler)
        {
            _enemy = enemy;
            _onEntered = onEntered;
        }

        public override void Enter()
        {
            Cursor.lockState = CursorLockMode.Confined;
            _enemy.MakeImmortal();

            _onEntered?.Invoke();
        }

        public override void ReportPlayerVictory()
        {

        }

        public override void ReportPlayerDeafeat()
        {

        }
    }
}
