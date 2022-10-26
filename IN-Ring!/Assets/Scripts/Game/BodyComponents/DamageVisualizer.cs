using UnityEngine;

namespace Game.BodyComponents
{
    public class DamageVisualizer : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Color _hitColor;
        [SerializeField] private float _speed;
        
        private Gradient _hitGradient;
        private bool _visualizing;
        private float _progress;

        private void Start()
        {
            _hitGradient = CreateGradient(_hitColor, _renderer.material.color);
        }

        private void Update()
        {
            if (_visualizing && _progress < 1f)
            {
                _renderer.material.color = _hitGradient.Evaluate(_progress);
                _progress += Time.deltaTime * _speed;
            }
            else
            {
                _visualizing = false;
                _progress = 0;
            }
        }

        public void Visualize()
        {
            _visualizing = true;
            _progress = 0;
        }

        private Gradient CreateGradient(Color startColor, Color endColor)
        {
            var result = new Gradient();
            var colorKeys = CreateColorKeys(startColor, endColor);
            var alphaKeys = CreateAlphaKeys(0, 1);

            result.SetKeys(colorKeys, alphaKeys);

            return result;
        }

        private GradientColorKey[] CreateColorKeys(Color startColor, Color endColor)
        {
            var colorKeys = new GradientColorKey[2];

            colorKeys[0].color = startColor;
            colorKeys[0].time = 0;
            colorKeys[1].color = endColor;
            colorKeys[1].time = 1;

            return colorKeys;
        }

        private GradientAlphaKey[] CreateAlphaKeys(float startAlpha, float endAlpha)
        {
            var alphaKeys = new GradientAlphaKey[2];

            alphaKeys[0].alpha = startAlpha;
            alphaKeys[0].time = 0;
            alphaKeys[1].alpha = endAlpha;
            alphaKeys[1].time = 1;

            return alphaKeys;
        }
    }
}