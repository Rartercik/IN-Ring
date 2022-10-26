using UnityEngine;
using UnityEngine.Events;

namespace Game.Utilities
{
    public class AnimationEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _animationEvent;

        private void Invoke()
        {
            _animationEvent?.Invoke();
        }
    }
}