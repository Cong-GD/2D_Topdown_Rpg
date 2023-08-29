using System.Collections.Generic;
using UnityEngine;

public class ChestStorage : GlobalReference<ChestStorage>
{
    [SerializeField] private ChestSlot itemSlotPrefabs;
    [SerializeField] private Transform contentCanvas;

    private readonly List<ChestSlot> slots = new();

    private IItem[] _showingItems;

    public void ShowItems(IItem[] items)
    {
        if(items == null)
            return;

        ChangeCapacity(items.Length);
        _showingItems = items;
        for (int i = 0; i < items.Length; i++)
        {
            slots[i].PushItem(items[i]);
        }
    }

    private void OnDisable()
    {
        if (_showingItems == null)
            return;

        for (int i = 0; i < _showingItems.Length; i++)
        {
            _showingItems[i] = slots[i].Item;
        }
        _showingItems = null;
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
}
