public class ConsumableSlot : ItemSlot<IUsableItem>
{
    public override bool IsMeetSlotRequiment(IItem item)
    {
        return item is null or IUsableItem;
    }

    protected override void OnSlotRightCliked()
    {
        base.OnSlotRightCliked();
        UseItem();
    }

    public void UseItem()
    {
        if (IsSlotEmpty)
            return;

        Item.Use(PlayerController.Instance.Combat);
    }
}