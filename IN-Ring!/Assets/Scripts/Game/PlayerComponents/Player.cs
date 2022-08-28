using UnityEngine;
using UnityEngine.Events;
using Game.BodyComponents;

namespace Game.PlayerComponents
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Body _enemy;
        [SerializeField] private Canvas _victory;
        [SerializeField] private UnityEvent _onWon;

        private void Start()
        {
            _enemy.OnDead += ShowVictory;
        }

        private void ShowVictory()
        {
            _victory.enabled = true;
            _onWon?.Invoke();
        }
    }
}