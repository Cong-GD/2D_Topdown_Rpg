using CongTDev.AudioManagement;
using CongTDev.EventManagers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ItemSlot<T> : MonoBehaviour, IItemSlot, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDropHandler, IEndDragHandler, IDragHandler, IPointerClickHandler where T : class, IItem
{
    [SerializeField] protected Image iconUI;

    public IItem BaseItem => Item;
    public T Item { get; private set; }

    public bool IsSlotEmpty { get; private set; } = true;

    public bool HasIconUI => iconUI != null;

    public abstract bool IsMeetSlotRequiment(IItem item);
    protected virtual void OnItemGetIn(T item)
    {
        item.OnDestroy += ClearSlot;

        if(HasIconUI)
        {
            iconUI.gameObject.SetActive(true);
            iconUI.sprite = Item.Icon;
        }
        
    }
    protected virtual void OnItemGetOut(T item)
    {
        item.OnDestroy -= ClearSlot;
        if (HasIconUI)
        {
            iconUI.gameObject.SetActive(false);
        }
    }
    protected virtual void OnSlotRightCliked()
    {
        EventManager<IItemSlot>.RaiseEvent("OnSlotPointerExit", this);
    }
    protected virtual void OnSlotLeftCliked()
    {
        EventManager<IItemSlot>.RaiseEvent("OnSlotPointerExit", this);
    }

    public void ClearSlot()
    {
        PushItem(null);
    }

    public IItem PushItem(IItem item)
    {
        if (!IsSlotEmpty)
        {
            IsSlotEmpty = true;
            OnItemGetOut(Item);
        }
        var oldItem = Item;
        Item = item as T;
        if (Item is not null)
        {
            IsSlotEmpty = false;
            OnItemGetIn(Item);
        }
        return oldItem;
    }

    public bool TryPushItem(IItem item, out IItem oldItem)
    {
        if (!IsMeetSlotRequiment(item))
        {
            oldItem = null;
            return false;
        }
        oldItem = PushItem(item);
        return true;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging || IsSlotEmpty)
            return;

        EventManager<IItemSlot>.RaiseEvent("OnSlotPointerEnter", this);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        EventManager<IItemSlot>.RaiseEvent("OnSlotPointerExit", this);
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        EventManager<IItemSlot>.RaiseEvent("OnSlotDrop", this);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || IsSlotEmpty)
            return;

        EventManager<IItemSlot>.RaiseEvent("OnSlotBeginDrag", this);
        iconUI.gameObject.SetActive(false);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        EventManager<IItemSlot>.RaiseEvent("OnSlotEndDrag", this);

        if (!IsSlotEmpty)
        {
            iconUI.gameObject.SetActive(true);
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Play("ItemSlotClick");
        if (IsSlotEmpty)
            return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnSlotRightCliked();
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnSlotLeftCliked();
        }
    }

    public void OnDrag(PointerEventData eventData) { }

}
