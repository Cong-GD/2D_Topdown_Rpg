using CongTDev.EventManagers;
using CongTDev.IOSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public const string TRY_ADD_ITEM_TO_INVENTROY = "OnTryAddItemToInventory";
    public const int DEFAULT_CAPACITY = 30;

    [SerializeField] private Transform contentCanvas;
    [SerializeField] private InventorySlot itemSlotPrefabs;
    [SerializeField] private List<BaseItemFactory> defaultIntialItems;
    [SerializeField] private OptionBox optionBox;
    [SerializeField] private TextMeshProUGUI goldText;

    private readonly List<InventorySlot> slots = new();

    public int Capacity => slots.Count;

    private void Awake()
    {
        InventorySlot.SetOptionBox(optionBox);
        SubscribeEvents();
    }

    private void OnEnable()
    {
        UpdateGoldValue();
        GameManager.OnGoldChange += UpdateGoldValue;
    }

    private void OnDisable()
    {
        optionBox.Disable();
        GameManager.OnGoldChange -= UpdateGoldValue;
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
        {
            ConfirmPanel.Ask("Your inventory is full now!");
            return;
        }

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

    public bool AddItem(IItem item)
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

    private void UpdateGoldValue()
    {
        goldText.text = $"Gold : {GameManager.PlayerGold}";
    }

    private void InitDefault()
    {
        ChangeCapacity(DEFAULT_CAPACITY);
        for (int i = 0; i < defaultIntialItems.Count && i < Capacity; i++)
        {
            slots[i].PushItem(defaultIntialItems[i].CreateItem());
        }
    }

    private void SaveInventory()
    {
        SaveLoadHandler.SaveToFile(FileNameData.Inventory, 
            ItemArray.GetIntance(slots.AsEnumerable().Select(slot => slot.Item).ToArray()));
    }
    private void LoadInventory()
    {
        try
        {
            var items = ((ItemArray)SaveLoadHandler.LoadFromFile(FileNameData.Inventory)).items;
            ChangeCapacity(items.Length);
            for (int i = 0; i< Capacity;i++)
            {
                slots[i].PushItem(items[i]);
            }
        }
        catch 
        {
            InitDefault();
        }
    }

}
