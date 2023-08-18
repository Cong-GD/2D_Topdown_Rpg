using CongTDev.EventManagers;
using CongTDev.IOSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public const string TRY_ADD_ITEM_TO_INVENTROY = "OnTryAddItemToInventory";

    [SerializeField] private Transform contentCanvas;
    [SerializeField] private InventorySlot itemSlotPrefabs;
    [SerializeField] private int _capacity;

    private readonly List<InventorySlot> slots = new();

    public int Capacity
    {
        get
        {
            return _capacity;
        }
        set
        {
            if (value < 0)
                return;

            ChangeCapacity(value);
            _capacity = value;
        }
    }

    private void Awake()
    {
        ChangeCapacity(Capacity);
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        EventManager<IItemSlot>.AddListener(TRY_ADD_ITEM_TO_INVENTROY, AddItemFromSlot);
        EventManager.AddListener("OnGameSave", SaveInventory);
        EventManager.AddListener("OnGameLoad", LoadInventory);
    }

    private void UnsubscribeEvents()
    {
        EventManager<IItemSlot>.RemoveListener(TRY_ADD_ITEM_TO_INVENTROY, AddItemFromSlot);
        EventManager.RemoveListener("OnGameSave", SaveInventory);
        EventManager.RemoveListener("OnGameLoad", LoadInventory);
    }

    public InventorySlot GetEmptySlot()
    {
        foreach (var slot in slots)
        {
            if (slot.IsSlotEmpty)
            {
                return slot;
            }
        }
        return null;
    }

    private void AddItemFromSlot(IItemSlot sourceSlot)
    {
        if (sourceSlot.BaseItem is IStackableItem source && TryStackItem(source))
            return;

        InventorySlot emptySlot = GetEmptySlot();
        if (emptySlot == null)
            return;

        IItemSlot.Swap(sourceSlot, emptySlot);
    }

    private bool TryStackItem(IStackableItem source)
    {
        foreach (var stackableItem in InventorySlot.onStackableItem)
        {
            if (IStackableItem.TryStackItem(source, stackableItem))
            {
                return true;
            }
        }
        return false;
    }

    private bool AddItem(IItem item)
    {
        InventorySlot emptySlot = GetEmptySlot();
        if (emptySlot == null)
            return false;

        return emptySlot.TryPushItem(item, out _);
    }

    private void ChangeCapacity(int newCapacity)
    {
        if (newCapacity < 0)
            return;

        while (slots.Count < newCapacity)
        {
            var newSlot = Instantiate(itemSlotPrefabs, contentCanvas);
            newSlot.PushItem(null);
            slots.Add(newSlot);
        }

        while (slots.Count > newCapacity)
        {
            int lastIndex = slots.Count - 1;
            var lastSLot = slots[lastIndex];
            slots.RemoveAt(lastIndex);
            Destroy(lastSLot.gameObject);
        }
    }

    #region IOSystem
    private void SaveInventory()
    {
        SaveLoadHandler.SaveToFile(FileNameData.Inventory, new SerializedInventory(this));
    }
    private void LoadInventory()
    {
        var serializedInventory = (SerializedInventory)SaveLoadHandler.LoadFromFile(FileNameData.Inventory);
        serializedInventory.Load(this);
    }

    [Serializable]
    public class SerializedInventory : SerializedObject, ISerializable
    {
        public string[] items;

        public SerializedInventory() { }

        public SerializedInventory(Inventory inventory)
        {
            var capacity = inventory.slots.Count;
            items = new string[capacity];

            for (int i = 0; i < capacity; i++)
            {
                items[i] = inventory.slots[i].Item.ToWrappedJson();
            }
        }

        public override SerializedType GetSerializedType() => SerializedType.Inventory;

        public void Load(Inventory inventory)
        {
            int capacity = items.Length;
            inventory.Capacity = capacity;
            for (int i = 0; i < capacity; i++)
            {
                var item = (IItem)JsonHelper.WrappedJsonToObject(items[i]);
                inventory.slots[i].PushItem(item);
            }
        }

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
