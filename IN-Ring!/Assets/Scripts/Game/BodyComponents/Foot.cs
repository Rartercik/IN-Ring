using UnityEngine;
using Game.Tools;

namespace Game.BodyComponents
{
    public class Foot : MonoBehaviour
    {
        [SerializeField] private LayerMask _floor;

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
    }
}