using CongTDev.IOSystem;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CongTDev.MainMenu
{
    public class RegisterPanel : MonoBehaviour
    {
        private const string UsernamePattern = @"^[a-zA-Z0-9_\-\s]+$";

        private static readonly string[] Error =
        {
            "This username is already exits",
            "Username has invalid charactor"
        };

        [SerializeField] private TextMeshProUGUI messageText;

        [SerializeField] private GameObject messagePanel;

        [SerializeField] private TMP_InputField inputField;

        [SerializeField] private int usernameLengthLimit;

        private HashSet<string> users;

        private void Awake()
        {
            inputField.characterLimit = usernameLengthLimit;
        }

        private void OnEnable()
        {
            users = new HashSet<string>(FileNameData.GetAllUser());
            inputField.text = string.Empty;
        }

        private void OnDisable()
        {
            users = null;
        }

        public void OnValueChange()
        {
            CheckInput(inputField.text);
        }

        public void OnSubmitButtonClick()
        {
            if (CheckInput(inputField.text))
            {
                ConfirmPanel.Ask(
                    "Username can't be changed.\n" +
                    "Do you want to start game with this username?",
                    StartGame);
            }
        }

        private void ShowMessage(int index)
        {
            messagePanel.SetActive(true);

            if (index < 0 || index >= Error.Length)
            {
                messageText.text = "Unknow error";
                return;
            }
            messageText.text = Error[index];
        }

        private void HideMessage()
        {
            messagePanel.SetActive(false);
        }

        private bool CheckInput(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                HideMessage();
                return false;
            }
            if (users.Contains(input))
            {
                ShowMessage(0);
                return false;
            }
            if (!Regex.IsMatch(input, UsernamePattern))
            {
                ShowMessage(1);
                return false;
            }
            HideMessage();
            return true;
        }

        private void StartGame()
        {
            FileNameData.AddUser(inputField.text);
            FileNameData.SetUser(inputField.text);
            SceneManager.LoadSceneAsync("CutScene");
        }
    }
}