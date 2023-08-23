using CongTDev.AudioManagement;
using CongTDev.IOSystem;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace CongTDev.MainMenu
{
    public class MainMenuController : GlobalReference<MainMenuController>
    {
        public void StartGameWithUserName(string username)
        {
            FileNameData.SetUser(username);
            SceneManager.LoadScene("Game");
        }

        private void Start()
        {
            AudioManager.Play("MenuBackgroundMusic").WhileTrue();
        }

        public void QuitGame()
        {
            ConfirmPanel.Instance.AskForComfirm(QuitGameWithoutConfirm, null, "Are you sure you want to quit game?");
        }

        private void QuitGameWithoutConfirm()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}