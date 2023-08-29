using System;
using TMPro;
using UnityEngine;

public class ConfirmPanel : MonoBehaviour
{
    private static Action<string, Action, Action> _instanceCallback;

    public static void Ask(string message, Action yesAction = null, Action noAction = null)
    {
        if(_instanceCallback != null)
        {
            _instanceCallback.Invoke(message, yesAction, noAction);
        }
        else
        {
            yesAction?.Invoke();
        }
    }

    [SerializeField] private TextMeshProUGUI messageText;

    private Action _yesAction;

    private Action _noAction;

    private void Awake()
    {
        _instanceCallback = AskForConfirmInternal;
    }

    private void AskForConfirmInternal(string message, Action yesAction, Action noAction)
    {
        gameObject.SetActive(true);
        messageText.text = message;
        this._yesAction = yesAction;
        this._noAction = noAction;
        Time.timeScale = 0f;
        InputCentral.Disable();
    }

    public void YesButtonClicked()
    {
        Cancel();
        _yesAction?.Invoke();
    }

    public void NoButtonClicked()
    {
        Cancel();
        _noAction?.Invoke();
    }

    public void Cancel()
    {
        Time.timeScale = 1f;
        InputCentral.Enable();
        gameObject.SetActive(false);
    }
}
