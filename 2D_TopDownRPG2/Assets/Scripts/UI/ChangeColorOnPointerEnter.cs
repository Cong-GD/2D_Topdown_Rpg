using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ChangeColorOnPointerEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color enterColor;
    [SerializeField] private Color exitColor;

    [SerializeField] private Image image;

    private void Reset()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = enterColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = exitColor;
    }
}
