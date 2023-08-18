using System.Collections.Generic;
using UnityEngine;

public class SetUpIntance : MonoBehaviour
{
    [SerializeField] private List<GameObject> intances;

    private void Awake()
    {
        foreach (GameObject intance in intances)
        {
            if(!intance.activeSelf)
            {
                intance.SetActive(true);
                intance.SetActive(false);
            }
        }
    }
}
