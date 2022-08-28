using UnityEngine;

namespace Game.BodyComponents
{
    public class DamageApplier : MonoBehaviour
    {
        [SerializeField] private Body _body;

        public void ApplyDamage(int damage)
        {
            _body.ApplyDamage(damage);
        }
    }
}
