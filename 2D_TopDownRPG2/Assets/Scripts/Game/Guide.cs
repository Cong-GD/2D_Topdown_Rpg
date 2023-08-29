using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Guide : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private List<GameObject> guides;

    private int _currentGuide = 0;

    private void Start()
    {
        if (guides.Count == 0)
            Destroy(gameObject);

        guides[0].gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        InputCentral.Enable();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MoveToMextGuide();
    }

    private void MoveToMextGuide()
    {
        guides[_currentGuide++].gameObject.SetActive(false);

        if (_currentGuide >= guides.Count)
        {
            Destroy(gameObject);
        }
        else
        {
            guides[_currentGuide].gameObject.SetActive(true);
        }
    }
}
