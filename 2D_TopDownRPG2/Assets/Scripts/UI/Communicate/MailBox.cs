using System.Collections.Generic;
using UnityEngine;

namespace CongTDev.Communicate
{
    public class MailBox : GlobalReference<MailBox>
    {
        [SerializeField] private MailBoxDislayer dislayer;

        [SerializeField] private MailContainer mailContainerPrefab;
        [SerializeField] private Transform containerParent;

        [SerializeField] private List<MailContainer> mailContainers;

        public void ReceiveMail(Mail mail)
        {
            var mailContainer = Instantiate(mailContainerPrefab);
            mailContainer.transform.SetParent(containerParent);
            mailContainers.Add(mailContainer);
            mailContainer.HoldingMail = mail;
        }

        public void ReadContainer(MailContainer container)
        {
            if (container.HoldingMail == null)
                return;

            dislayer.DisplayMail(container.HoldingMail);
        }
    }
}