public interface IStackableItem : IItem
{
    int Capacity { get; }
    int Count { get; }
    int Stack(int amount);
    int Get(int amount);

    bool IsFullStacked();

    bool CanStackWith(IStackableItem stackable);

    static bool TryStackItem(IStackableItem source, IStackableItem dest)
    {
        if (!dest.IsFullStacked() && dest.CanStackWith(source))
        {
            int stackAmount = dest.Capacity - dest.Count;
            dest.Stack(source.Get(stackAmount));
            return true;
        }
        return false;
    }
}
