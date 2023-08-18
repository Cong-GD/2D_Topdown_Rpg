public interface IItemSlot
{
    IItem BaseItem { get; }

    bool IsSlotEmpty { get; }

    IItem PushItem(IItem item);

    void ClearSlot();

    bool TryPushItem(IItem item, out IItem oldItem);

    bool IsMeetSlotRequiment(IItem item);

    static bool Swap(IItemSlot sourceSlot, IItemSlot destSlot)
    {
        if (!sourceSlot.IsMeetSlotRequiment(destSlot.BaseItem) || !destSlot.IsMeetSlotRequiment(sourceSlot.BaseItem))
            return false;

        var item1 = sourceSlot.PushItem(null);
        var item2 = destSlot.PushItem(null);
        sourceSlot.PushItem(item2);
        destSlot.PushItem(item1);
        return true;
    }
}
