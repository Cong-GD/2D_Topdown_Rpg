using CongTDev.EventManagers;
using CongTDev.ObjectPooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStageSpawner : MonoBehaviour
{
    [SerializeField] private List<MonsterSpawnInfo> monsterLists;
    [SerializeField] private Prefab spawningEffect;
    [SerializeField] private float spawnRange;
    [SerializeField] private float spawnDelay;

    public int CurrentMonster { get; private set; } = 0;

    public void StartSpawning(Action onSpawningEnded)
    {
        StartCoroutine(SpawnCoroutine(onSpawningEnded));
    }

    private IEnumerator SpawnCoroutine(Action onSpawningEnded)
    {
        foreach (MonsterSpawnInfo spawnInfo in monsterLists)
        {
            SpawnMonsters(spawnInfo);
            yield return new WaitUntil(() => CurrentMonster <= 0);
        }
        onSpawningEnded?.Invoke();
    }

    public void SpawnMonsters(MonsterSpawnInfo spawnInfo)
    {
        const int MAX_TRY_ALLOW = 100;
        for (int count = 0, tried = 0; count < spawnInfo.count && tried < MAX_TRY_ALLOW;)
        {
            if(SpawnInRandomPosition(spawnInfo))
            {
                count++;
            }
            else
            {
                tried++;
            }
        }
    }

    public bool SpawnInRandomPosition(MonsterSpawnInfo spawnInfo)
    {
        var position = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * spawnRange;
        if(!CanSpawnAtThisPoint(position))
            return false;

        StartCoroutine(SpawnMonster(spawnInfo, position));
        return true;
    }

    public IEnumerator SpawnMonster(MonsterSpawnInfo spawnInfo , Vector2 spawnPosition)
    {
        CurrentMonster++;
        if (PoolManager.Get<PoolObject>(spawningEffect, out var effect))
        {
            effect.transform.position = spawnPosition;
            yield return spawnDelay.Wait();
            effect.ReturnToPool();
        }

        if(PoolManager.Get<MonstersController>(spawnInfo.prefab, out var monster))
        {
            monster.transform.position = spawnPosition;
            monster.Initialize(spawnInfo.statData, spawnInfo.level);
            monster.StageSupportDeathEvent += _ => CurrentMonster--;
            if(spawnInfo.xp > 0)
            {
                monster.StageSupportDeathEvent += _ => EventManager<int>.RaiseEvent("OnReceiveXp", spawnInfo.xp);
            }
        }
        else
        {
            CurrentMonster--;
        }
    }

    private bool CanSpawnAtThisPoint(Vector2 position)
    {
        return !Physics2D.OverlapCircle(position, 1f, LayerMaskHelper.ObstacleMask);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    } 
#endif

}

[Serializable]
public class MonsterSpawnInfo
{
    public Prefab prefab;
    public BaseStatData statData;
    public int level;
    public int count;
    public int xp;
}

