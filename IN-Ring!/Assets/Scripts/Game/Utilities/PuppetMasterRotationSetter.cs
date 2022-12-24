using UnityEngine;
using RootMotion.Dynamics;

namespace Game.Utilities
{
    public class PuppetMasterRotationSetter : MonoBehaviour
    {
        [SerializeField] private Transform _puppetMasterTransform;
        [SerializeField] private PuppetMaster _puppetMaster;
        [SerializeField] private Quaternion _rotation;

        private void Awake()
        {
            _puppetMaster.Teleport(_puppetMasterTransform.position, _rotation, false);
        }
    }
}