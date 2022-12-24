using System;
using UnityEngine;
using Game.BodyComponents;

namespace Game.FightingEnvironment.GameStates
{
    public class PlayerVictory : GameState
    {
        private readonly Body _player;
        private readonly Action _onEntered;

        public PlayerVictory(GameStatesHandler statesHandler, Body player, Action onEntered) : base(statesHandler)
        {
            _player = player;
            _onEntered = onEntered;
        }

        public override void Enter()
        {
            Cursor.lockState = CursorLockMode.Confined;
            _player.MakeImmortal();

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
