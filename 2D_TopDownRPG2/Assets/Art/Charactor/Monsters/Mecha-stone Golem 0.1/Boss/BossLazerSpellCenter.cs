using CongTDev.AbilitySystem;
using CongTDev.AbilitySystem.Spell;
using CongTDev.ObjectPooling;
using UnityEngine;

public class BossLazerSpellCenter : PoolObject, ISpell
{
    [SerializeField] private Prefab lazerSpellPrefab;

    [Min(1)]
    [SerializeField] private int count;

    public void KickOff(OrientationAbility ability, Vector2 direction)
    {
        var directions = VectorHelper.SpreadDirectionAdd(direction, count, 360);

        for (int i = 0; i < count; i++)
        {
            if (PoolManager.Get<BossLazerSpell>(lazerSpellPrefab, out var lazerSpell))
            {
                lazerSpell.SetBehaviour(BossLazerSpell.Behaviour.RotateAround);
                lazerSpell.KickOff(ability, directions[i]);

            }
        }
        ReturnToPool();
    }
}
