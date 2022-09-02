using UnityEngine;

namespace Game.SoundEffects
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomPitchSource : MonoBehaviour
    {
        [SerializeField] private AudioClip _shot;
        [SerializeField] private float _minPitch;
        [SerializeField] private float _maxPitch;

        private AudioSource _source;

        private void Start()
        {
            _source = GetComponent<AudioSource>();
        }

        public void PlayOneShot()
        {
            _source.pitch = Random.Range(_minPitch, _maxPitch);
            _source.PlayOneShot(_shot);
        }
    }
}