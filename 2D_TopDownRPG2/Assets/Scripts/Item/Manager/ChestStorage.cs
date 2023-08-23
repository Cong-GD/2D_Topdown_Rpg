using System.Collections.Generic;
using UnityEngine;

public class ChestStorage : GlobalReference<ChestStorage>
{
    [SerializeField] private ChestSlot itemSlotPrefabs;
    [SerializeField] private Transform contentCanvas;

    private readonly List<ChestSlot> slots = new();

    private IItem[] showingItems;

    public void ShowItem(IItem[] items)
    {
        ChangeCapacity(items.Length);
        showingItems = items;
        for (int i = 0; i < items.Length; i++)
        {
            slots[i].PushItem(items[i]);
        }
    }

    private void OnDisable()
    {
        if (showingItems == null)
            return;

        for (int i = 0; i < showingItems.Length; i++)
        {
            showingItems[i] = slots[i].Item;
        }
        showingItems = null;
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
