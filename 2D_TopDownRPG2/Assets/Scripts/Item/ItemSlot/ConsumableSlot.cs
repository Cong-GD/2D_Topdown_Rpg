using TMPro;
using UnityEngine;

public class ConsumableSlot : ItemSlot<ConsumableItem>
{
    [SerializeField] private TextMeshProUGUI amountText;

    public override bool IsMeetSlotRequiment(IItem item)
    {
        return item is null or ConsumableItem;
    }

    protected override void OnItemGetIn(ConsumableItem item)
    {
        base.OnItemGetIn(item);
        amountText.transform.parent.gameObject.SetActive(true);
        item.OnCountChange += UpdateUI;
        UpdateUI();
    }

    protected override void OnItemGetOut(ConsumableItem item)
    {
        base.OnItemGetOut(item);
        item.OnCountChange -= UpdateUI;
        amountText.transform.parent.gameObject.SetActive(false);
    }

    protected override void OnSlotRightCliked()
    {
        base.OnSlotRightCliked();
        UseItem();
    }

    private void UpdateUI()
    {
        if (IsSlotEmpty)
            return;

        amountText.text = Item.Count.ToString();
    }

    public void UseItem()
    {
        if (IsSlotEmpty)
            return;

        Item.Use(PlayerController.Instance.Combat);
    }
}