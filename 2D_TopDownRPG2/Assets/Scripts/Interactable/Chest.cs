using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool IsOpening { get; private set; }
    [SerializeField] private Sprite openingChest;
    [SerializeField] private Sprite closingChest;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        if (IsOpening)
        {
            _renderer.sprite = closingChest;
            IsOpening = false;
            return;
        }
        _renderer.sprite = openingChest;
        IsOpening = true;
    }
}
