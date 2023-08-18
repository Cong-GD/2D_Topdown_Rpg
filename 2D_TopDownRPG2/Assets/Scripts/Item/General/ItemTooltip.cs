using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void ShowItemToolTip(IItem item)
    {
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
}
