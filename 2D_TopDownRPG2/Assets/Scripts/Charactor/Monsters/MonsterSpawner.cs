using CongTDev.ObjectPooling;
using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private const float CHECK_INTERVAL = 2f;

    [SerializeField] private MonstersController monsterPrefab;
    [SerializeField] private BaseStatData monsterStatData;

    [SerializeField] private float spawnRange;
    public int maxAmount;
    [SerializeField] private int initialAmount;
    [SerializeField] private float spawnDelay;

    private ObjectPool _pool;

    public int MaxAmount 
    {
        get => maxAmount;
        set
        {
            if (value < 0)
                value = 0;

            maxAmount = value;
        }
    }

    private void Awake()
    {
        _pool = new ObjectPool(monsterPrefab.gameObject);
        for (int i = 0; i < initialAmount; i++)
        {
            SpawnInRandomPosition();
        }
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        var nextSpawnTime = Time.time + spawnDelay;
        while(true)
        {
            if(_pool.CountActive < maxAmount && Time.time > nextSpawnTime)
            {
                const int maxTryTime = 15;
                int tried = 0;
                while(tried++ < maxTryTime)
                {
                    if (SpawnInRandomPosition())
                        break;
                }
                nextSpawnTime = Time.time + spawnDelay;
            }
            yield return CHECK_INTERVAL.Wait();
        }
    }

    public bool SpawnInRandomPosition()
    {
        var position = (Vector2)transform.position + Random.insideUnitCircle * spawnRange;
        return SpawnMonster(position);
    }

    public bool SpawnMonster(Vector2 spawnPosition)
    {
        if (Physics2D.OverlapPoint(spawnPosition, LayerMaskHelper.ObstacleMask))
            return false;

        var monster = (MonstersController)_pool.Get();
        monster.Initialize(monsterStatData);
        monster.transform.position = spawnPosition;
        return true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    } 
#endif
}