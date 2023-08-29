using CongTDev.AudioManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickSound : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Play("ButtonClick");
    }
}
