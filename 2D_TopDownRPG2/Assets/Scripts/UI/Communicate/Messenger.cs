using CongTDev.EventManagers;
using CongTDev.IOSystem;
using CongTDev.ObjectPooling;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CongTDev.Communicate
{
    public class Messenger : MonoBehaviour
    {
        public const string SEND_SYSTEM_MESSAGE = "SendSystemMessage";

        [SerializeField] private TMP_InputField playerInput;

        [SerializeField] private Message messagePrefab;

        [SerializeField] private Transform contentPanel;

        private ObjectPool messagePool;

        private Queue<Message> activeMessage;

        private void Awake()
        {
            activeMessage = new Queue<Message>();
            messagePool = new ObjectPool(messagePrefab.gameObject);
            EventManager<string>.AddListener(SEND_SYSTEM_MESSAGE, SentMessageFromSystem);
        }

        private void OnDestroy()
        {
            EventManager<string>.RemoveListener(SEND_SYSTEM_MESSAGE, SentMessageFromSystem);
        }

        public void SendMessageFromPlayer()
        {
            if (string.IsNullOrEmpty(playerInput.text))
                return;

            if(CheatCode.IsCheatAllow)
            {
                CheatCode.TryApplyCheat(playerInput.text);
            }
            SentMessageFromUser((FileNameData.CurrentUser, playerInput.text));
            playerInput.text = string.Empty;
        }

        private void SentMessageFromSystem(string message)
        {
            var messageUI = GetMessage();
            messageUI.SetSystemMessage(message);
        }

        private void SentMessageFromUser((string sender, string message) t)
        {
            var messageUI = GetMessage();
            messageUI.SetMessage(t.sender, t.message);
        }

        private Message GetMessage()
        {
            var messageUI = (Message)messagePool.Get();
            messageUI.transform.SetParent(contentPanel);
            activeMessage.Enqueue(messageUI);
            TrimMessage(20);
            return messageUI;
        }

        public void OnSelected()
        {
            InputCentral.Disable();
        }
        public void OnDeselected()
        {
            InputCentral.Enable();
        }

        private void TrimMessage(int newSize)
        {
            if (newSize < 0)
                return;

            while (activeMessage.Count > newSize)
            {
                var message = activeMessage.Dequeue();
                message.ReturnToPool();
            }
        }
    }
}