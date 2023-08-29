using CongTDev.AbilitySystem;
using CongTDev.AbilitySystem.Spell;
using CongTDev.AudioManagement;
using CongTDev.ObjectPooling;
using UnityEngine;

namespace CongTDev.TheBoss
{
    public class BossHandFireCenter : PoolObject, ISpell
    {
        [SerializeField] private Prefab handFirePrefab;
        [SerializeField] private int projectileCount;

        public void KickOff(OrientationAbility ability, Vector2 _)
        {
            var firePosition = ability.Caster.transform.parent.Find("HandFirePosition").transform;
            if (firePosition != null)
            {
                transform.position = firePosition.position;
            }
            else
            {
                transform.position = ability.Caster.Owner.HitBox.bounds.center;
            }
            AudioManager.Play("SmallExplosion");
            for (int i = 0; i < projectileCount; i++)
            {
                var direction = Random.insideUnitCircle;
                if (PoolManager.Get<StraightBullet>(handFirePrefab, out var handFire))
                {
                    handFire.KickOff(ability, direction);
                    handFire.transform.position = transform.position;
                }
            }
            ReturnToPool();
        }
    }
}