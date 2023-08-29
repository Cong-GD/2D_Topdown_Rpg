using CongTDev.AudioManagement;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject openingDoor;
    [SerializeField] private GameObject closingDoor;

    private bool _isOpenning;

    private void Awake()
    {
        _isOpenning = openingDoor.gameObject.activeSelf;
    }


    public void SwicthState()
    {
        if(_isOpenning)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
        AudioManager.Play("DoorOpenClose");
    }

    public void CloseDoor()
    {
        openingDoor.SetActive(false);
        closingDoor.SetActive(true);
        _isOpenning = false;
    }

    public void OpenDoor()
    {
        openingDoor.SetActive(true);
        closingDoor.SetActive(false);
        _isOpenning = true;
    }
}
