using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    [SerializeField] private List<ShopSlot> shopSlots;
    [SerializeField] private List<ShopItem> randomItems;
    [SerializeField] private bool isRandom;
    [SerializeField] private int minAmount;

    [Serializable]
    public class ShopItem
    {
        public BaseItemFactory factory;
        public int minPrice;
        public int maxPrice;
    }

    private void Awake()
    {
        if (isRandom)
        {
            InitItemsRandomly();
        }
        else
        {
            InitItems();
        }
    }

    private void InitItems()
    {
        for (int i = 0; i < shopSlots.Count && i < randomItems.Count; i++)
        {
            var shopItem = randomItems[i];
            var price = UnityEngine.Random.Range(shopItem.minPrice, shopItem.maxPrice + 1);
            shopSlots[i].PushItem(shopItem.factory.CreateItem());
            shopSlots[i].SetPrice(price);
        }
    }

    private void InitItemsRandomly()
    {
        int randomNumberOfSlot = UnityEngine.Random.Range(Mathf.Clamp(minAmount, 0, shopSlots.Count -1), shopSlots.Count + 1);
        for (int i = 0; i < randomNumberOfSlot; i++)
        {
            var randomItemIndex = UnityEngine.Random.Range(0, randomItems.Count);
            var shopItem = randomItems[randomItemIndex];
            var price = UnityEngine.Random.Range(shopItem.minPrice, shopItem.maxPrice + 1);
            shopSlots[i].PushItem(shopItem.factory.CreateItem());
            shopSlots[i].SetPrice(price);
        }
    }
}
