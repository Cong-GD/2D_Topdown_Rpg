using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Item/ConsumableRandomAmount")]
public class RandomAmountConsumableFactory : BaseItemFactory
{
    [SerializeField] private ConsumableItemFactory consumableFactory;

    [SerializeField] private int minAmount;
    [SerializeField] private int maxAmount;

    public override IItem CreateItem()
    {
        var item = consumableFactory.CreateItem() as ConsumableItem;
        if (maxAmount < minAmount)
        {
            Debug.LogWarning("Max amount can't less than min amount");
        }
        else
        {
            item.Stack(Random.Range(minAmount, maxAmount) - 1);
        }
        return item;
    }

}
