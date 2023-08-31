using CongTDev.AbilitySystem;
using CongTDev.AudioManagement;
using CongTDev.EventManagers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlot : ItemSlot<IItem>
{
    [SerializeField] private TextMeshProUGUI priceText;

    private int _price;

    private void OnEnable()
    {
        GameManager.OnGoldChange += UpdatePriceUI;
        UpdatePriceUI();
    }

    private void OnDisable()
    {
        GameManager.OnGoldChange -= UpdatePriceUI;
    }

    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void OnDrop(PointerEventData eventData) 
    {
        if (!IsSlotEmpty)
            return;

        var holder = new ObjectHolder<IItemSlot>();
        EventManager<ObjectHolder<IItemSlot>>.RaiseEvent("RequestCurrentDraggingSlot", holder);
        if (holder.value is null || holder.value.IsSlotEmpty)
            return;

        if(holder.value.BaseItem is IAbility)
        {
            ConfirmPanel.Ask("You can't sell learned ability");
            return;
        }

        ConfirmPanel.Ask("Do you want to sold this item with 1G?", 
            () =>
            {
                if (IItemSlot.Swap(this, holder.value))
                {
                    GameManager.PlayerGold += 1;
                    AudioManager.Play("BuySell");
                    SetPrice(50);
                }
            });
    }

    public override bool IsMeetSlotRequiment(IItem item)
    {
        return true;
    }

    public void SetPrice(int price)
    {
        _price = price;
        UpdatePriceUI();
    }

    protected override void OnItemGetIn(IItem item)
    {
        base.OnItemGetIn(item);
        priceText.gameObject.SetActive(true);
    }

    protected override void OnItemGetOut(IItem item)
    {
        base.OnItemGetOut(item);
        priceText.gameObject.SetActive(false);
    }

    protected override void OnSlotRightCliked()
    {
        base.OnSlotRightCliked();
        if(GameManager.PlayerGold < _price)
        {
            ConfirmPanel.Ask("Don't have enough gold!");
        }
        else
        {
            ConfirmPanel.Ask($"Do you want to buy this item with price {_price}G",
                () =>
                {
                    EventManager<IItemSlot>.RaiseEvent(Inventory.TRY_ADD_ITEM_TO_INVENTROY, this);
                    if(!IsSlotEmpty)
                    {
                        ConfirmPanel.Ask("Don't have enough space in your inventory!");
                    }
                    else
                    {
                        GameManager.PlayerGold -= _price;
                        AudioManager.Play("BuySell");
                    }
                });
        }
    }
    private void UpdatePriceUI()
    {
        if(GameManager.PlayerGold < _price)
        {
            priceText.text = $"{_price} G".RichText(Color.red);
        }
        else
        {
            priceText.text = $"{_price} G";
        }
    }
}