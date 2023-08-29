using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = transform.position;
    }
}
