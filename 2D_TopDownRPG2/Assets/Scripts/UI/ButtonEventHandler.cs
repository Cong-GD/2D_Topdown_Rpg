using CongTDev.EventManagers;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonEventHandler : MonoBehaviour
{
    [SerializeField] private string eventName;

    [SerializeField] private Button button;

    private void Reset()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(RaiseEvent);
    }

    private void RaiseEvent()
    {

        EventManager.RaiseEvent(eventName);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(RaiseEvent);
    }
}