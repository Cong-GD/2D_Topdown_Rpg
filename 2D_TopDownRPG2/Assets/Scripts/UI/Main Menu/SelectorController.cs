using CongTDev.EventManagers;
using CongTDev.IOSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CongTDev.MainMenu
{
    public class SelectorController : MonoBehaviour
    {

        [SerializeField] private ToggleGroup toggleGroup;

        [SerializeField] private ToggleUserSeletion toggleUserSeletionPrefab;

        [SerializeField] private TextMeshProUGUI currentUserText;


        private readonly List<ToggleUserSeletion> toggleUserSeletions = new();

        public void RefreshUserToggle()
        {
            var users = FileNameData.GetAllUser();
            SetNewSize(users.Length);
            int index = 0;
            foreach (var user in users)
            {
                toggleUserSeletions[index++].SetUser(toggleGroup, user);
            }
        }

        public void DeleteCurrentUser()
        {
            if (string.IsNullOrEmpty(currentUserText.text))
                return;

            ConfirmPanel.Ask("Are you sure to delete this user?", DeleteCurrenUserExcute);
        }

        public void DeleteCurrenUserExcute()
        {
            FileNameData.DeleteUser(currentUserText.text);
            currentUserText.text = "";
            RefreshUserToggle();
        }

        public void OnSubmitButtonClick()
        {
            if (string.IsNullOrEmpty(currentUserText.text))
                return;

            FileNameData.SetUser(currentUserText.text);
            SceneManager.LoadSceneAsync("Game");
        }

        private void OnEnable()
        {
            EventManager<string>.AddListener("OnUserSelected", ChangeCurrentUser);
            currentUserText.text = string.Empty;
            RefreshUserToggle();
        }

        private void OnDisable()
        {
            EventManager<string>.RemoveListener("OnUserSelected", ChangeCurrentUser);
        }

        private void ChangeCurrentUser(string username)
        {
            currentUserText.text = username;
        }

        private void SetNewSize(int newSize)
        {
            if (newSize < 0)
                return;

            while (toggleUserSeletions.Count < newSize)
            {
                var toggle = Instantiate(toggleUserSeletionPrefab, toggleGroup.transform);
                toggleUserSeletions.Add(toggle);
            }
            while (toggleUserSeletions.Count > newSize)
            {
                var lassIndex = toggleUserSeletions.Count - 1;
                var lassToggle = toggleUserSeletions[lassIndex];
                toggleUserSeletions.RemoveAt(lassIndex);
                Destroy(lassToggle.gameObject);
            }
        }


    }
}