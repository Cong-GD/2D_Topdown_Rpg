using CongTDev.AudioManagement;
using UnityEngine;

public class LootChest : BaseInteractable
{
    public const int CAPACITY = 9;

    [SerializeField] private BaseItemFactory[] randomFactorys;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite unblockedSprite;
    [SerializeField] private Sprite emptyChestSprite;
    [SerializeField] private BaseIndicator unblockedIndicator;
    [SerializeField] private int minGoldReceive;
    [SerializeField] private int maxGoldReceive;

    private IItem[] _items;
    private bool _isOpening = false;
    private bool _isBlocked = true;
    private bool _receivedGold = false;

    public void Unblock()
    {
        _isBlocked = false;
        InitChestItem();
        spriteRenderer.sprite = unblockedSprite;
        indicator.Deactive();
        indicator = unblockedIndicator;
        if (PlayerInteract.IsCurrentAssined(this))
        {
            indicator.Active();
        }
    }

    public override void Interact()
    {
        if (_isBlocked)
            return;

        SetState(!_isOpening);
    }

    public override void OnCancelAssigned()
    {
        base.OnCancelAssigned();
        SetState(false);
    }

    private void InitChestItem()
    {
        _items = new IItem[CAPACITY];
        if (randomFactorys.Length == 0)
            return;

        var numberOfItem = Random.Range(1, CAPACITY + 1);
        int count = 0;
        int current = 0;

        while (count < numberOfItem && current < _items.Length)
        {
            int randomIndex = Random.Range(0, randomFactorys.Length);
            _items[current++] = randomFactorys[randomIndex].CreateItem();
        }
    }

    private void GiveGold()
    {
        if (_receivedGold || minGoldReceive <= 0)
            return;

        _receivedGold = true;
        int gold = Random.Range(minGoldReceive, maxGoldReceive);
        ConfirmPanel.Ask($"You have got {gold} G!", ReceiveGold, ReceiveGold);

        void ReceiveGold()
        {
            AudioManager.Play("BuySell");
            GameManager.PlayerGold += gold;
        }
    }

    private void SetState(bool state)
    {
        var chestStogare = ChestStorage.Instance;
        if (chestStogare == null)
            return;

        _isOpening = state;
        chestStogare.gameObject.SetActive(_isOpening);
        if (_isOpening)
        {
            GiveGold();
            spriteRenderer.sprite = emptyChestSprite;
            chestStogare.ShowItems(_items);
        }
    }
}
