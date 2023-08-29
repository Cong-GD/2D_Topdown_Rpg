using CongTDev.AudioManagement;
using UnityEngine;

public class TownDoor : BaseInteractable
{
    [SerializeField] private GameObject openingDoor;
    [SerializeField] private GameObject closingDoor;

    [SerializeField] private bool defaultOpen;

    private bool _isOpening;

    public bool IsOpening
    {
        get => _isOpening;
        set
        {
            openingDoor.SetActive(value);
            closingDoor.SetActive(!value);
            _isOpening = value;
        }
    }

    private void Awake()
    {
        IsOpening = defaultOpen;
    }

    public override void Interact()
    {
        IsOpening = !IsOpening;
        AudioManager.Play("DoorOpenClose");
    }
}
