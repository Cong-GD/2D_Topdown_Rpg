using CongTDev.EventManagers;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemOptionBox : MonoBehaviour
{
    public ItemOption[] itemOptions;

    private void Awake()
    {
        EventManager<IEnumerable<KeyValuePair<string, Action>>>.AddListener("ShowInventoryItemFunction", ShowItemFunctions);
    }

    private void OnDestroy()
    {
        EventManager<IEnumerable<KeyValuePair<string, Action>>>.RemoveListener("ShowInventoryItemFunction", ShowItemFunctions);
    }

    private void ShowItemFunctions(IEnumerable<KeyValuePair<string, Action>> actionMap)
    {
        DisableAllButton();
        int i = 0;
        foreach (KeyValuePair<string, Action> pair in actionMap)
        {
            if (i > itemOptions.Length)
                return;

            itemOptions[i].ShowOption(pair.Key, pair.Value);
            itemOptions[i].ClickAction += DisableAllButton;
            i++;
        }
    }

    private void DisableAllButton()
    {
        foreach (var itemOption in itemOptions)
        {
            itemOption.gameObject.SetActive(false);
        }
    }
}
