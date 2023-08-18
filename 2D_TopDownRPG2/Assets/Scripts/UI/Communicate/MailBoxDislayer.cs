using System;
using TMPro;
using UnityEngine;

namespace CongTDev.Communicate
{
    public class MailBoxDislayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI bodyText;
        [SerializeField] private RewardSlot[] rewardSlots;

        private Mail _displayingMail;

        private void Start()
        {
            if (rewardSlots.Length != Mail.ATTACHED_SLOT_LIMIT)
            {
                throw new ArgumentException("Reward slot's number not equals limit attached slot", nameof(MailBoxDislayer));
            }
            for (int i = 0; i < Mail.ATTACHED_SLOT_LIMIT; i++)
            {
                rewardSlots[i].SlotIndex = i;
                rewardSlots[i].OnItemGot += OnItemGot;
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < Mail.ATTACHED_SLOT_LIMIT; i++)
            {
                rewardSlots[i].OnItemGot -= OnItemGot;
            }
        }

        public void DisplayMail(Mail mail)
        {
            _displayingMail = null;
            titleText.text = mail.Title;
            bodyText.text = mail.Body;
            for (int i = 0; i < Mail.ATTACHED_SLOT_LIMIT; i++)
            {
                rewardSlots[i].PushItem(mail.AttachedItems[i]);
            }
            _displayingMail = mail;
        }

        private void OnItemGot(int index)
        {
            if (_displayingMail == null)
                return;

            _displayingMail.AttachedItems[index] = null;
        }
    }
}