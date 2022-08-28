using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Interface
{
    public class Restarter : MonoBehaviour
    {
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}