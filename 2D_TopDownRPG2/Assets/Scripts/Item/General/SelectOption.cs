using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectOption : MonoBehaviour
{
    [SerializeField]
    private Button button;

    [SerializeField]
    private TextMeshProUGUI text;

    public event Action ClickAction;

    private void Awake()
    {
        button.onClick.AddListener(CallbackAction);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(CallbackAction);
    }

    public void ShowOption(string name, Action action)
    {
        gameObject.SetActive(true);
        text.text = name;
        ClickAction = action;
    }

    public void CallbackAction()
    {
        ClickAction?.Invoke();
        ClickAction = null;
    }
}
