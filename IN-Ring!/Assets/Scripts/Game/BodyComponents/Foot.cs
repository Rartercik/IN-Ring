using UnityEngine;
using Game.Tools;

namespace Game.BodyComponents
{
    public class Foot : MonoBehaviour
    {
        [SerializeField] private LayerMask _floor;
        [SerializeField] private Collider _collider;
        [SerializeField] private PhysicMaterial _zeroFriction;
        [SerializeField] private PhysicMaterial _maxFriction;

        public bool OnFloor { get; private set; }

        private void OnCollisionEnter(Collision other)
        {
            if (LayerTool.EqualLayers(_floor, other.gameObject.layer))
            {
                OnFloor = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (LayerTool.EqualLayers(_floor, other.gameObject.layer))
            {
                OnFloor = false;
            }
        }

        public void SetZeroFriction()
        {
            _collider.material = _zeroFriction;
        }

        public void SetMaxFriction()
        {
            _collider.material = _maxFriction;
        }
    }
}