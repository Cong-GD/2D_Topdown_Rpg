using CongTDev.EventManagers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CongTDev.MainMenu
{
    public class ToggleUserSeletion : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI userText;

        [SerializeField] private Toggle toggle;

        private void OnEnable()
        {
            toggle.onValueChanged.AddListener(ChangeCurrentUser);
        }

        private void OnDisable()
        {
            toggle.onValueChanged.RemoveListener(ChangeCurrentUser);
        }

        public void SetUser(ToggleGroup toggleGroup, string userName)
        {
            userText.text = userName;
            toggle.group = toggleGroup;
        }

        private void ChangeCurrentUser(bool isToggleOn)
        {
            if (isToggleOn && userText.text != "")
            {
                EventManager<string>.RaiseEvent("OnUserSelected", userText.text);
            }
        }
    }
}