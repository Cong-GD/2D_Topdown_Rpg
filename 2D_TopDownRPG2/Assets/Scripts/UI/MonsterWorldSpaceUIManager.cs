using CongTDev.ObjectPooling;
using UnityEngine;

public class MonsterWorldSpaceUIManager : GlobalReference<MonsterWorldSpaceUIManager>
{
    [SerializeField] private Prefab healthBarPrefab;
    [SerializeField] private Prefab followMonsterInfo;
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

    public FollowMonsterInfo GetMonsterInfo(MonstersController monstersController)
    {
        if(PoolManager.Get<FollowMonsterInfo>(followMonsterInfo, out var monsterInfo))
        {
            monsterInfo.transform.SetParent(worldSpaceCanvas);
            monsterInfo.AttachToMonster(monstersController);
        }
        return monsterInfo;
    }
}