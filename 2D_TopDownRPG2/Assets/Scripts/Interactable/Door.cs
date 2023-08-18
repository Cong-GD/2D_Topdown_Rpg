using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject openingDoor;
    [SerializeField] private GameObject closingDoor;

    public bool isOpening;

    private void Awake()
    {
        Switch();
    }

    public void Interact()
    {
        isOpening = !isOpening;
        Switch();
    }

    public void Switch()
    {
        openingDoor.SetActive(isOpening);
        closingDoor.SetActive(!isOpening);
    }
}
