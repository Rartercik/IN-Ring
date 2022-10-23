using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pursuer : MonoBehaviour
{
    [SerializeField] private Rigidbody _target;

    [Space(30)]
    [Header("Required Components:")]
    [Space(5)]
    [SerializeField] private Rigidbody _rigidbody;

    [Button]
    private void SetRequiredComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.position = _target.position;
    }
}
