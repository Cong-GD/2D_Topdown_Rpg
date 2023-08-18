using CongTDev.IOSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : IItem, IStackableItem, IUsableItem
{
    public const string ITEM_TYPE = "Consumable";

    private readonly ConsumableItemFactorySO sourceItem;

    public ConsumableItem(ConsumableItemFactorySO sourceItem)
    {
        this.sourceItem = sourceItem;
        Count = 1;
    }

    public event Action OnDestroy;

    public Sprite Icon => sourceItem.Icon;

    public ItemRarity Rarity => sourceItem.ItemRarity;

    public string Name => sourceItem.ItemName;

    public int Capacity => sourceItem.MaxStack;

    public int Count { get; private set; }

    public string GetDescription() => sourceItem.Description;

    public string ItemType => ITEM_TYPE;

    public IEnumerable<string> GetSubTypes() => sourceItem.Types;

    public void Use(Fighter user)
    {
        foreach (var effectBuilder in sourceItem.Effects)
        {
            user.ReceiveEffect(effectBuilder, user);
        }
        Get(1);
    }

    public bool IsFullStacked() => Count >= Capacity;

    public int Stack(int amount)
    {
        if (amount <= 0)
        {
            return 0;
        }
        int prefit = Count + amount;
        if (prefit > Capacity)
        {
            amount -= prefit - Capacity;
            Count = Capacity;
        }
        Count += amount;
        return amount;
    }

    public int Get(int amount)
    {
        if (amount <= 0)
            return 0;

        int prefit = Count - amount;
        if (prefit <= 0)
        {
            amount += prefit;
            Destroy();
        }
        Count -= amount;
        return amount;
    }

    public bool CanStackWith(IStackableItem stackable)
    {
        if (stackable is not ConsumableItem consumableItem)
            return false;

        return sourceItem == consumableItem.sourceItem;
    }

    public void Destroy()
    {
        OnDestroy?.Invoke();
    }


    #region IOSystem
    public SerializedObject Serialize()
    {
        return new SerializedConsumableItem(this);
    }

    public class SerializedConsumableItem : SerializedObject
    {
        public string sourceItemJson;
        public int count;
        public SerializedConsumableItem(ConsumableItem consumableItem)
        {
            count = consumableItem.Count;
            sourceItemJson = consumableItem.sourceItem.ToWrappedJson();
        }

        public override object Deserialize()
        {
            var sourceInfo = (ConsumableItemFactorySO)JsonHelper.WrappedJsonToObject(sourceItemJson);
            var consumableItem = new ConsumableItem(sourceInfo)
            {
                Count = count
            };
            return consumableItem;
        }

        public override SerializedType GetSerializedType() => SerializedType.ConsumableItem;
    }
    #endregion
}