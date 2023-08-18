using CongTDev.ObjectPooling;
using TMPro;
using UnityEngine;

namespace CongTDev.Communicate
{
    public class Message : PoolObject
    {
        [SerializeField] private TextMeshProUGUI textUI;

        public void SetMessage(string sender, string message)
        {
            textUI.text = $"{sender} : {message}";
        }

        public void SetSystemMessage(string message)
        {
            textUI.text = $"[System] {message}";
        }
    }
}