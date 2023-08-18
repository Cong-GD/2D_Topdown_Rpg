using CongTDev.AbilitySystem;
using CongTDev.EventManagers;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PassiveAbilitySlot : ItemSlot<IPassiveAbility>
{
    [SerializeField] private Image background;

    public override bool IsMeetSlotRequiment(IItem item)
    {
        return item is null or IPassiveAbility;
    }

    protected override void OnItemGetIn(IPassiveAbility passiveAbility)
    {
        base.OnItemGetIn(passiveAbility);
        PlayerController.Instance.AbilityCaster.AddPassiveAbility(passiveAbility);
        if(background != null)
        {
            background.enabled = false;
        }
    }

    protected override void OnItemGetOut(IPassiveAbility passiveAbility)
    {
        base.OnItemGetOut(passiveAbility);
        PlayerController.Instance.AbilityCaster.RemovePassiveAbility(passiveAbility);
        if (background != null)
        {
            background.enabled = true;
        }
    }

    protected override void OnSlotRightCliked()
    {
        EventManager<IItemSlot>.RaiseEvent(Inventory.TRY_ADD_ITEM_TO_INVENTROY, this);
    }
}


