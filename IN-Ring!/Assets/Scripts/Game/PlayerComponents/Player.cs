using UnityEngine;
using Game.Interface;
using Game.FightingEnvironment;
using Game.BodyComponents;

namespace Game.PlayerComponents
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Body _enemy;
        [SerializeField] private GameStatesHandler _gameStatesHandler;
        [SerializeField] private InputPC _input;

        private void Start()
        {
            _enemy.OnDead += InitializeVictory;
        }

        private void InitializeVictory()
        {
            _gameStatesHandler.ReportPlayerVictory();
            _input.enabled = false;
        }
    }
}