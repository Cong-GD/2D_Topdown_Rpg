using CongTDev.AbilitySystem;
using CongTDev.EventManagers;
using CongTDev.IOSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        try
        {
            var serializedEquipmentsManager = (SerializedEquipmentManager)SaveLoadHandler.LoadFromFile(FileNameData.Equiments);
            serializedEquipmentsManager.Load(this);
        }
        catch 
        {
            foreach (var slot in equipmentSlotsMap.Values)
            {
                slot.PushItem(null);
            }
            foreach (var slot in passiveAbilitySlots)
            {
                slot.PushItem(null);
            }
        }
        
    }

    public class SerializedEquipmentManager : SerializedObject, ISerializable
    {
        public JsonWrapper weaponJson;
        public JsonWrapper shieldJson;
        public JsonWrapper shoseJson;
        public JsonWrapper arrmorJson;
        public JsonWrapper[] passiveAbilityJsons;

        public SerializedEquipmentManager() { }

        public SerializedEquipmentManager(EquipmentManager equipmentManager)
        {
            weaponJson = equipmentManager.equipmentSlotsMap[Equipment.Slot.Weapon].Item.ToJsonWrapper();
            shieldJson = equipmentManager.equipmentSlotsMap[Equipment.Slot.Shield].Item.ToJsonWrapper();
            shoseJson = equipmentManager.equipmentSlotsMap[Equipment.Slot.Shoe].Item.ToJsonWrapper();
            arrmorJson = equipmentManager.equipmentSlotsMap[Equipment.Slot.Armor].Item.ToJsonWrapper();
            passiveAbilityJsons = equipmentManager.passiveAbilitySlots.Select(slot => slot.Item.ToJsonWrapper()).ToArray();
        }

        public void Load(EquipmentManager equipmentManager)
        {
            var weapon = (Equipment)weaponJson.ToObject();
            var shield = (Equipment)shieldJson.ToObject();
            var shose = (Equipment)shoseJson.ToObject();
            var arrmor = (Equipment)arrmorJson.ToObject();
            equipmentManager.equipmentSlotsMap[Equipment.Slot.Weapon].PushItem(weapon);
            equipmentManager.equipmentSlotsMap[Equipment.Slot.Shield].PushItem(shield);
            equipmentManager.equipmentSlotsMap[Equipment.Slot.Shoe].PushItem(shose);
            equipmentManager.equipmentSlotsMap[Equipment.Slot.Armor].PushItem(arrmor);

            var passiveAbilities = new PassiveAbility[passiveAbilityJsons.Length];
            for (int i = 0; i < passiveAbilities.Length; i++)
            {
                passiveAbilities[i] = (PassiveAbility)passiveAbilityJsons[i].ToObject();
                equipmentManager.passiveAbilitySlots[i].PushItem(passiveAbilities[i]);
            }
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
