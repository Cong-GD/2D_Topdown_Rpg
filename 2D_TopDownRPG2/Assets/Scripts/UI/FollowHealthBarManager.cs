using CongTDev.ObjectPooling;
using UnityEngine;

public class FollowHealthBarManager : GlobalReference<FollowHealthBarManager>
{
    [SerializeField] private Prefab healthBarPrefab;
    [SerializeField] private Transform worldSpaceCanvas;

    public FollowHealthBar ReceiveHealthBar(Fighter fighter)
    {
        if(PoolManager.Get<FollowHealthBar>(healthBarPrefab, out var healthBar))
        {
            healthBar.transform.SetParent(worldSpaceCanvas);
            healthBar.AttachToFighter(fighter);
        }
        return healthBar;
    }
}