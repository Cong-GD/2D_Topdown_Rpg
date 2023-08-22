using CongTDev.EventManagers;
using UnityEngine;

public class ToggleSwitch : MonoBehaviour
{
    public void Switch()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
