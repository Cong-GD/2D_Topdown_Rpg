﻿using CongTDev.EventManagers;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemEventHandler : MonoBehaviour
{
    [SerializeField] private ItemTooltip itemToolTip;
    [SerializeField] private Image draggingImage;

    private IItemSlot _onDraggingSlot;
    private bool _isDragging;

    private void Awake()
    {
        SubscribeEvent();
    }

    private void OnDestroy()
    {
        UnsubscribeEvent();
    }

    private void SubscribeEvent()
    {
        EventManager<IItemSlot>.AddListener("OnSlotPointerEnter", ShowTip);
        EventManager<IItemSlot>.AddListener("OnSlotPointerExit", HideTip);

        EventManager<IItemSlot>.AddListener("OnSlotBeginDrag", OnSlotBeginDrag);
        EventManager<IItemSlot>.AddListener("OnSlotDrop", OnItemDrop);
        EventManager<IItemSlot>.AddListener("OnSlotEndDrag", OnSlotEndDrag);
    }

    private void UnsubscribeEvent()
    {
        EventManager<IItemSlot>.RemoveListener("OnSlotPointerEnter", ShowTip);
        EventManager<IItemSlot>.RemoveListener("OnSlotPointerExit", HideTip);

        EventManager<IItemSlot>.RemoveListener("OnSlotBeginDrag", OnSlotBeginDrag);
        EventManager<IItemSlot>.RemoveListener("OnSlotDrop", OnItemDrop);
        EventManager<IItemSlot>.RemoveListener("OnSlotEndDrag", OnSlotEndDrag);
    }

    private void ShowTip(IItemSlot itemSlot)
    {
        itemToolTip.gameObject.SetActive(true);
        itemToolTip.ShowItemToolTip(itemSlot.BaseItem);
        itemToolTip.transform.position = Input.mousePosition;
    }

    private void HideTip(IItemSlot _)
    {
        itemToolTip.gameObject.SetActive(false);
    }

    private void OnSlotBeginDrag(IItemSlot itemSlot)
    {
        draggingImage.gameObject.SetActive(true);
        draggingImage.sprite = itemSlot.BaseItem.Icon;
        _onDraggingSlot = itemSlot;
        _isDragging = true;
        StartCoroutine(DragingRoutine(itemSlot));
    }

    private IEnumerator DragingRoutine(IItemSlot itemSlot)
    {
        var waits = new WaitForEndOfFrame();
        while (_isDragging)
        {
            draggingImage.transform.position = Input.mousePosition;
            yield return waits;
        }
    }

    private void OnItemDrop(IItemSlot itemSlot)
    {
        if (!_isDragging)
            return;

        if (_onDraggingSlot.BaseItem is IStackableItem source && itemSlot.BaseItem is IStackableItem dest && IStackableItem.TryStackItem(source, dest))
            return;

        if (IItemSlot.Swap(_onDraggingSlot, itemSlot))
        {
            OnSlotEndDrag(_onDraggingSlot);
        }
    }

    private void OnSlotEndDrag(IItemSlot _)
    {
        draggingImage.gameObject.SetActive(false);
        _onDraggingSlot = null;
        _isDragging = false;
    }

}
