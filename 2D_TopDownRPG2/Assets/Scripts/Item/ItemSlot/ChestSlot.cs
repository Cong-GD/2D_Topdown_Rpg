using CongTDev.EventManagers;

public class ChestSlot : ItemSlot<IItem>
{
    public override bool IsMeetSlotRequiment(IItem item)
    {
        return true;
    }

    protected override void OnSlotRightCliked()
    {
        base.OnSlotRightCliked();
        EventManager<IItemSlot>.RaiseEvent(Inventory.TRY_ADD_ITEM_TO_INVENTROY, this);
    }
}
