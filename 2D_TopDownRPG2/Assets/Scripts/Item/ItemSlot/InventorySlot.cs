using CongTDev.AbilitySystem;
using CongTDev.AudioManagement;
using CongTDev.EventManagers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

public class InventorySlot : ItemSlot<IItem>
{
    #region Static
    public static readonly List<IStackableItem> onStackableItem = new();

    private static OptionBox _optionBox;

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
           Rune.ITEM_TYPE , new Dictionary<string, Action<InventorySlot>>()
           {
               { "Learn", LearnAbility },
               { "Drop", Drop }
           }
        }
    };

    public static void SetOptionBox(OptionBox optionBox)
    {
        _optionBox = optionBox;
    }

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
        ConfirmPanel.Ask("Item will be deleted permanently.\nAre you sure?", slot.ClearSlot);
    }

    private static void EquipAbility(InventorySlot slot)
    {
        switch (slot.Item)
        {
            case IActiveAbility:
                EventManager<IItemSlot>.RaiseEvent("TryEquipActiveAbility", slot);
                break;
            case IPassiveAbility:
                EventManager<IItemSlot>.RaiseEvent("TryEquipPassiveAbility", slot);
                break;
        }
    }

    private static void LearnAbility(InventorySlot slot)
    {
        if (slot.Item is not Rune rune)
            return;


        if (GameManager.PlayerGold >= rune.LearnCost)
        {
            ConfirmPanel.Ask($"Do you want to learn this ability with cost: {rune.LearnCost} G",
                    () =>
                    {
                        GameManager.PlayerGold -= rune.LearnCost;
                        AudioManager.Play("BuySell");
                        slot.PushItem(rune.CreateItem());
                    });
        }
        else
        {
            ConfirmPanel.Ask("Don't have enought gold to learn this ability");
        }
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
        base.OnSlotRightCliked();
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
            StartCoroutine(SelectionCoroutine(actionMap));
        }
    }

    private IEnumerator SelectionCoroutine(IEnumerable<KeyValuePair<string, Action<InventorySlot>>> actionMap)
    {
        if (_optionBox == null)
            yield break;

        _optionBox.ShowOptions(actionMap.ToDictionary(x => x.Key, x => x.Value.WrapAction(this)));
        _optionBox.transform.position = transform.position;
        yield return null;
        while (_optionBox.IsShowing)
        {
            if (Mouse.current.press.wasReleasedThisFrame)
            {
                _optionBox.Disable();
            }
            yield return null;
        }

    }
}
