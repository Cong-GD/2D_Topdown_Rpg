using CongTDev.AbilitySystem;
using CongTDev.EventManagers;
using System;
using System.Collections.Generic;
using System.Linq;

public class InventorySlot : ItemSlot<IItem>
{
    #region Static
    public static readonly List<IStackableItem> onStackableItem = new();

    private static readonly IReadOnlyDictionary<string, IEnumerable<KeyValuePair<string, Action<InventorySlot>>>> _clickActionMaps
        = new Dictionary<string, IEnumerable<KeyValuePair<string, Action<InventorySlot>>>>()
    {
        {
            ConsumableItem.ITEM_TYPE , new Dictionary<string, Action<InventorySlot>>()
            {
                { "Use", UseItem },
                { "Drop", Drop }
            }
        },
        {
            Equipment.ITEM_TYPE , new Dictionary<string, Action<InventorySlot>>()
            {
                { "Equip", EquipEquipment },
                { "Drop", Drop }

            }
        },
        {
           IAbility.ITEM_TYPE , new Dictionary<string, Action<InventorySlot>>()
           {
               { "Equip", EquipAbility },
               { "Discard", Drop }
           }
        },
        {
           RuneSO.ITEM_TYPE , new Dictionary<string, Action<InventorySlot>>()
           {
               { "Acquire", AcquireAbility },
               { "Drop", Drop }
           }
        }
    };

    private static void UseItem(InventorySlot slot)
    {
        if (slot.Item is not ConsumableItem consumItem)
            return;

        consumItem.Use(PlayerController.Instance.Combat);
    }

    private static void EquipEquipment(InventorySlot slot)
    {
        EventManager<IItemSlot>.RaiseEvent("TryEquipEquipment", slot);
    }

    private static void Drop(InventorySlot slot)
    {
        slot.ClearSlot();
    }

    private static void EquipAbility(InventorySlot slot)
    {
        switch(slot.Item)
        {
            case IActiveAbility:
                EventManager<IItemSlot>.RaiseEvent("TryEquipActiveAbility", slot);
                break;
            case IPassiveAbility:
                EventManager<IItemSlot>.RaiseEvent("TryEquipPassiveAbility", slot);
                break;
        }
    }

    private static void AcquireAbility(InventorySlot slot)
    {
        if (slot.Item is not RuneSO rune)
            return;

        slot.PushItem(rune.CreateItem());
    } 
    #endregion

    public override bool IsMeetSlotRequiment(IItem item)
    {
        return true;
    }

    protected override void OnItemGetIn(IItem item)
    {
        base.OnItemGetIn(item);
        if (item is IStackableItem stackable)
        {
            onStackableItem.Add(stackable);
        }
    }

    protected override void OnItemGetOut(IItem item)
    {
        base.OnItemGetOut(item);
        if (item is IStackableItem stackable)
        {
            onStackableItem.Remove(stackable);
        }
    }

    protected override void OnSlotRightCliked()
    {
        if (_clickActionMaps.TryGetValue(Item.ItemType, out var actionPairs))
        {
            actionPairs.First().Value.Invoke(this);
        }
    }

    protected override void OnSlotLeftCliked()
    {
        base.OnSlotLeftCliked();
        if (_clickActionMaps.TryGetValue(Item.ItemType, out var actionMap))
        {
            var wrappedActionMap = actionMap.ToDictionary(x => x.Key, x => x.Value.WrapAction(this));
            EventManager<IEnumerable<KeyValuePair<string, Action>>>.RaiseEvent("ShowInventoryItemFunction", wrappedActionMap);
        }
    }

}
