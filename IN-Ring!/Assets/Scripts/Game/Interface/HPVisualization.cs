using UnityEngine;
using UnityEngine.UI;

namespace Game.Interface
{
    public class HPVisualization : MonoBehaviour
    {
        [SerializeField] private Image _HP;

        public void Visualize(int HP, int maxHP)
        {
            var fillAmount = (float)HP / maxHP;
            _HP.fillAmount = fillAmount;
        }
    }
}