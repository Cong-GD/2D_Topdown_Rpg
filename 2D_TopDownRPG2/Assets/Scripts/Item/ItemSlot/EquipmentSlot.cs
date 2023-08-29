using CongTDev.AudioManagement;
using CongTDev.EventManagers;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : ItemSlot<Equipment>
{
    [field: SerializeField] public Equipment.Slot EquipSlot { get; private set; }

    [SerializeField] private Image background;

    public override bool IsMeetSlotRequiment(IItem item)
    {
        return item is null || (item is Equipment equipment && equipment.EquipSlot == EquipSlot);
    }

    protected override void OnItemGetIn(Equipment item)
    {
        base.OnItemGetIn(item);
        item.Equip(PlayerController.Instance.Combat);
        AudioManager.Play("Equip");
        if (background != null)
        {
            background.enabled = false;
        }   
    }

    protected override void OnItemGetOut(Equipment item)
    {
        base.OnItemGetOut(item);
        item.Unequip();
        AudioManager.Play("Unequip");
        if (background != null)
        {
            background.enabled = true;
        }
    }

    protected override void OnSlotRightCliked()
    {
        base.OnSlotRightCliked();
        EventManager<IItemSlot>.RaiseEvent(Inventory.TRY_ADD_ITEM_TO_INVENTROY, this);
    }
}