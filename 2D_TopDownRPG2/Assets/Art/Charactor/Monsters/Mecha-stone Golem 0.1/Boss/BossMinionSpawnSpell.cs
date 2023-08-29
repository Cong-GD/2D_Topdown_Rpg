using CongTDev.AbilitySystem;
using CongTDev.AbilitySystem.Spell;
using CongTDev.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace CongTDev.TheBoss
{

    public class BossMinionSpawnSpell : PoolObject, ISpell
    {
        [SerializeField] private Prefab monionPrefab;
        [SerializeField] private Prefab spawnEffectPrefab;
        [SerializeField] private BaseStatData statData;
        [SerializeField] private float minRange;
        [SerializeField] private float maxRange;
        [SerializeField] private int count;
        [SerializeField] private float spawnDelay;

        public void KickOff(OrientationAbility ability, Vector2 direction)
        {
            StartCoroutine(SpawningCoroutine(ability.Caster.Owner.Position + direction));
        }

        private IEnumerator SpawningCoroutine(Vector2 position)
        {
            var spawnPositions = ListPool<Vector2>.Get();
            CalculateSpawnPoints(position, spawnPositions);
            yield return SpawnEffectState(spawnPositions);
            SpawnMonions(spawnPositions);
            ListPool<Vector2>.Release(spawnPositions);
            ReturnToPool();
        }

        private void CalculateSpawnPoints(Vector2 position, List<Vector2> spawnPositions)
        {
            for (int i = 0; i < count; i++)
            {
                var spawnPosition = position + Random.insideUnitCircle * Random.Range(minRange, maxRange);
                spawnPositions.Add(spawnPosition);
            }
        }


        private IEnumerator SpawnEffectState(List<Vector2> spawnPositions)
        {
            var listEffect = ListPool<IPoolObject>.Get();
            foreach (var spawnPosition in spawnPositions)
            {
                if (PoolManager.Get<PoolObject>(spawnEffectPrefab, out var intance))
                {
                    intance.transform.position = spawnPosition;
                    listEffect.Add(intance);
                }
            }
            yield return spawnDelay.Wait();

            foreach (var effect in listEffect)
            {
                effect.ReturnToPool();
            }
            ListPool<IPoolObject>.Release(listEffect);
        }

        private void SpawnMonions(List<Vector2> spawnPositions)
        {
            foreach (var spawnPosition in spawnPositions)
            {
                if (PoolManager.Get<MonstersController>(monionPrefab, out var minion))
                {
                    minion.transform.position = spawnPosition;
                    minion.Initialize(statData, PlayerLevelSystem.CurrentLevel);
                }
            }
        }
    }
}