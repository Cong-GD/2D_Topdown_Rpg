using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CongTDev.Communicate
{
    public class MailContainer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Image mask;
        [SerializeField] private Color readedColor;
        [SerializeField] private Color notReadedColor;
        [SerializeField] private Color hightlightColor;

        private Mail _holdingMail;

        public bool IsReaded { get; private set; }

        public Mail HoldingMail
        {
            get => _holdingMail;
            set
            {
                _holdingMail = value;
                titleText.text = value.Title;
                IsReaded = false;
                ValidateColor();
            }
        }
        private void OnEnable()
        {
            ValidateColor();
        }

        public void ReadThisMail()
        {
            IsReaded = true;
            MailBox.Instance.ReadContainer(this);
            ValidateColor();
        }

        public void Highlight()
        {
            mask.color = hightlightColor;
        }

        public void ValidateColor()
        {
            mask.color = IsReaded ? readedColor : notReadedColor;
        }
    }
}