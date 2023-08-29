using CongTDev.EventManagers;
using CongTDev.IOSystem;
using System;
using UnityEngine;
using UnityEngine.Pool;

public class TownChest : BaseInteractable
{
    public const int CHEST_CAPACITY = 9;

    [SerializeField] private Sprite openingChest;
    [SerializeField] private Sprite closingChest;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BaseItemFactory[] defaultInitialItems;

    [Space]
    [Header("Save handler")]
    [SerializeField] private string saveFileName;

    private IItem[] _items;

    private bool _isOpening;
    public bool IsOpening
    {
        get => _isOpening;
        set
        {
            spriteRenderer.sprite = value ? openingChest : closingChest;
            _isOpening = value;
            CheckForOpenChest();
        }
    }

    private void Awake()
    {
        EventManager.AddListener("OnGameSave", SaveStogare);
        EventManager.AddListener("OnGameLoad", LoadStogare);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener("OnGameSave", SaveStogare);
        EventManager.RemoveListener("OnGameLoad", LoadStogare);
    }

    private void LoadStogare()
    {
        try
        {
            _items = ((ItemArray)SaveLoadHandler.LoadFromFile(saveFileName)).items;
        }
        catch
        {
            InitDefault();
        }
    }

    private void SaveStogare()
    {
        SaveLoadHandler.SaveToFile(saveFileName, ItemArray.GetIntance(_items));
    }

    public override void Interact()
    {
        IsOpening = !IsOpening;
    }

    public override void OnCancelAssigned()
    {
        base.OnCancelAssigned();
        IsOpening = false;
    }

    private void CheckForOpenChest()
    {
        var chestStogare = ChestStorage.Instance;
        if (chestStogare == null)
            return;

        chestStogare.gameObject.SetActive(IsOpening);
        if (IsOpening)
        {
            chestStogare.ShowItems(_items);
        }
    }

    private void InitDefault()
    {
        _items = new IItem[CHEST_CAPACITY];
        for (int i = 0; i < defaultInitialItems.Length && i < CHEST_CAPACITY; i++)
        {
            _items[i] = defaultInitialItems[i].CreateItem();
        }
    }

}
