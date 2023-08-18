using System;
using TMPro;
using UnityEngine;

public class ConfirmPanel : GlobalReference<ConfirmPanel>
{
    [SerializeField] private TextMeshProUGUI messageText;

    private Action yesAction;

    private Action noAction;

    public void AskForComfirm(Action yesAction, Action noAction, string message)
    {
        gameObject.SetActive(true);
        messageText.text = message;
        this.yesAction = yesAction;
        this.noAction = noAction;
    }

    public void YesButtonClicked()
    {
        yesAction?.Invoke();
        Cancel();
    }

    public void NoButtonClicked()
    {
        noAction?.Invoke();
        Cancel();
    }

    public void Cancel()
    {
        yesAction = null;
        noAction = null;
        gameObject.SetActive(false);
    }
}
