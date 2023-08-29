using UnityEngine;

namespace CongTDev.MainMenu
{
    public class QuitGameHelper : MonoBehaviour
    {
        public void QuitGame()
        {
            Game.Quit();
        }
    }
}