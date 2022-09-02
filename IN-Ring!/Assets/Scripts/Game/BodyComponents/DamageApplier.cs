using UnityEngine;
using Game.SoundEffects;

namespace Game.BodyComponents
{
    [RequireComponent(typeof(DamageVisualizer))]
    [RequireComponent(typeof(RandomPitchSource))]
    public class DamageApplier : MonoBehaviour
    {
        [SerializeField] private Body _body;
        [SerializeField] private Rigidbody _rigidbody;

        private DamageVisualizer _visualizer;
        private RandomPitchSource _soundSource;

        public Rigidbody Rigidbody => _rigidbody;

        private void Start()
        {
            _visualizer = GetComponent<DamageVisualizer>();
            _soundSource = GetComponent<RandomPitchSource>();
        }

        public void ApplyDamage(int damage)
        {
            _body.ApplyDamage(damage);
            _visualizer.Visualize();
            _soundSource.PlayOneShot();
        }
    }
}
