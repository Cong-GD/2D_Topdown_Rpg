using CongTDev.ObjectPooling;
using UnityEngine;

namespace CongTDev.AbilitySystem.Spell
{
    public class SpikeSpellCenter : PoolObject, ISpell
    {
        [SerializeField] private Prefab spikeSpellPrefab;
        [SerializeField] private float minRange;
        [SerializeField] private float maxRange;
        [SerializeField] private Transform[] spawnPoints;

        public void KickOff(OrientationAbility ability, Vector2 _)
        {
            foreach (var spawnPoint in spawnPoints)
            {
                if (PoolManager.Get<SpikeSpell>(spikeSpellPrefab, out var spikeSpell))
                {
                    spikeSpell.KickOff(ability, spawnPoint.position);
                }
            }
        }
    }
}

