using CongTDev.AbilitySystem;
using CongTDev.EventManagers;
using CongTDev.IOSystem;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private EquipmentSlot[] equipmentSlot;

    [SerializeField] private PassiveAbilitySlot[] passiveAbilitySlots;

    private readonly Dictionary<Equipment.Slot, EquipmentSlot> equipmentSlotsMap = new();

    private void Awake()
    {
        foreach (var slot in equipmentSlot)
        {
            equipmentSlotsMap.TryAdd(slot.EquipSlot, slot);
        }
        SubscribeEvents();
    }
    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
    private void SubscribeEvents()
    {
        EventManager<IItemSlot>.AddListener("TryEquipEquipment", TryEquipEquipment);
        EventManager<IItemSlot>.AddListener("TryEquipPassiveAbility", TryToEquipPassiveAbility);
        EventManager.AddListener("OnGameSave", SaveEquipment);
        EventManager.AddListener("OnGameLoad", LoadEquipment);
    }

    private void UnsubscribeEvents()
    {
        EventManager<IItemSlot>.RemoveListener("TryEquipEquipment", TryEquipEquipment);
        EventManager<IItemSlot>.RemoveListener("TryEquipPassiveAbility", TryToEquipPassiveAbility);
        EventManager.RemoveListener("OnGameSave", SaveEquipment);
        EventManager.RemoveListener("OnGameLoad", LoadEquipment);
    }

    private void TryEquipEquipment(IItemSlot sourceSlot)
    {
        if (sourceSlot.BaseItem is not Equipment equipment)
            return;

        if (equipmentSlotsMap.TryGetValue(equipment.EquipSlot, out var equipmentSlot))
        {
            IItemSlot.Swap(equipmentSlot, sourceSlot);
        }
    }

    private void TryToEquipPassiveAbility(IItemSlot slot)
    {
        if (slot.BaseItem is not IPassiveAbility)
            return;

        foreach (var passiveSlot in passiveAbilitySlots)
        {
            if (passiveSlot.IsSlotEmpty)
            {
                IItemSlot.Swap(slot, passiveSlot);
            }
        }
    }

    #region IOSystem
    private void SaveEquipment()
    {
        SaveLoadHandler.SaveToFile(FileNameData.Equiments, new SerializedEquipmentManager(this));
    }

    private void LoadEquipment()
    {
        var serializedEquipmentsManager = (SerializedEquipmentManager)SaveLoadHandler.LoadFromFile(FileNameData.Equiments);
        serializedEquipmentsManager.Load(this);
    }

    public class SerializedEquipmentManager : SerializedObject, ISerializable
    {
        public string weaponJson;
        public string shieldJson;

        public SerializedEquipmentManager() { }

        public SerializedEquipmentManager(EquipmentManager equipmentManager)
        {
            weaponJson = equipmentManager.equipmentSlotsMap[Equipment.Slot.Weapon].Item.ToWrappedJson();
            shieldJson = equipmentManager.equipmentSlotsMap[Equipment.Slot.Shield].Item.ToWrappedJson();
        }

        public void Load(EquipmentManager equipmentManager)
        {
            var weapon = (Equipment)JsonHelper.WrappedJsonToObject(weaponJson);
            var shield = (Equipment)JsonHelper.WrappedJsonToObject(shieldJson);
            equipmentManager.equipmentSlotsMap[Equipment.Slot.Weapon].PushItem(weapon);
            equipmentManager.equipmentSlotsMap[Equipment.Slot.Shield].PushItem(shield);
        }

        public override SerializedType GetSerializedType() => SerializedType.EquipmentManager;

        public SerializedObject Serialize()
        {
            return this;
        }

        public override object Deserialize()
        {
            return this;
        }
    }
    #endregion
}
