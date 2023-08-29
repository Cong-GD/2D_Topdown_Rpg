using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Space]
    [Header("Pivot attribute")]
    [SerializeField] private RectTransform thisRectTramsfrom;
    [SerializeField] private float leftPoint;
    [SerializeField] private float rightPoint;
    [SerializeField] private float topPoint;
    [SerializeField] private float bottomPoint;

    public void ShowItemToolTip(IItem item)
    {
        transform.position = Input.mousePosition;

        thisRectTramsfrom.pivot = GetGoodPivotPoint();

        Color rarityColor = ColorHelper.GetRarityColor(item.Rarity);

        nameText.text = item.Name.RichText(rarityColor);
        rarityText.text = item.Rarity.ToString().RichText(rarityColor);
        typeText.text = $"{item.ItemType.RichText(item.GetItemTypeColor())}\n{string.Join(", ", item.GetSubTypes())}";
        descriptionText.text = item.GetDescription();

        if(item is IStackableItem stackable)
        {
            quantityText.text = $"Quantity: {stackable.Count} / {stackable.Capacity}";
        }
        else
        {
            quantityText.text = string.Empty;
        }
    }

    private Vector2 GetGoodPivotPoint()
    {
        var x = transform.position.x < Screen.width / 2 ? leftPoint : rightPoint;
        var y = transform.position.y < Screen.height / 2 ? bottomPoint : topPoint;
        return new Vector2(x, y);
    }

}
