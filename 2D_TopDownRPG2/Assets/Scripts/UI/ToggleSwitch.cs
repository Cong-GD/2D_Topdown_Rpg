using CongTDev.EventManagers;
using UnityEngine;

public class ToggleSwitch : MonoBehaviour
{
    [SerializeField] private string eventTrigger;

    private void Awake()
    {
        if (string.IsNullOrEmpty(eventTrigger))
            return;

        EventManager.AddListener(eventTrigger, Switch);
    }

    private void OnDestroy()
    {
        if (string.IsNullOrEmpty(eventTrigger))
            return;

        EventManager.RemoveListener(eventTrigger, Switch);
    }

    public void Switch()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
