using CongTDev.AbilitySystem;
using CongTDev.AbilitySystem.Spell;
using CongTDev.ObjectPooling;
using UnityEngine;

public class BossStormSpellCenter : PoolObject, ISpell
{
    [SerializeField] private Prefab stormSpellPrefab;
    [SerializeField] private float minRange;
    [SerializeField] private float maxRange;
    [SerializeField] private int count;

    public void KickOff(OrientationAbility ability, Vector2 _)
    {
        var casterPosition = ability.Caster.Owner.Position;
        for (int i = 0; i < count; i++)
        {
            if (PoolManager.Get<BossStormSpell>(stormSpellPrefab, out var stormSpell))
            {
                var direction = Random.Range(minRange, maxRange) * Random.insideUnitCircle;
                stormSpell.KickOff(ability, casterPosition + direction);
            }
        }
        ReturnToPool();
    }
}
