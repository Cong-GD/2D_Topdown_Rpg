using CongTDev.Communicate;
using CongTDev.EventManagers;
using System;

public class RewardSlot : ItemSlot<IItem>
{
    public int SlotIndex { get; set;}

    public event Action<int> OnItemGot;

    public override bool IsMeetSlotRequiment(IItem item)
    {
        return item is null;
    }

    protected override void OnItemGetOut(IItem item)
    {
        base.OnItemGetOut(item);
        OnItemGot?.Invoke(SlotIndex);
    }

    protected override void OnSlotRightCliked()
    {
        base.OnSlotRightCliked();
        EventManager<IItemSlot>.RaiseEvent(Inventory.TRY_ADD_ITEM_TO_INVENTROY, this);
        if(!IsSlotEmpty)
        {
            EventManager<string>.RaiseEvent(Messenger.SEND_SYSTEM_MESSAGE, "Your inventory is full now. Please clean up it first.");
        }
    }
}