using CongTDev.AbilitySystem;
using CongTDev.AbilitySystem.Spell;
using CongTDev.AudioManagement;
using CongTDev.ObjectPooling;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace CongTDev.TheBoss
{
    public class BossDamageZoneSpell : PoolObject, ISpell
    {
        [SerializeField] private Prefab ropePrefab;
        [SerializeField] private Prefab minionSpawnSpellPrefab;
        [SerializeField] private GameObject explosion;
        [SerializeField] private GameObject indicator;
        [SerializeField] private Rigidbody2D rb2d;

        [SerializeField] private float moveSpeed;
        [SerializeField] private float changeDirectionInterval;
        [SerializeField] private float activeDelay;
        [SerializeField] private float damageRange;
        [SerializeField] private float blockTime;

        public void KickOff(OrientationAbility ability, Vector2 position)
        {
            transform.position = position;
            indicator.SetActive(true);
            StartCoroutine(SpellCoroutine(ability));
        }

        private IEnumerator SpellCoroutine(OrientationAbility ability)
        {
            var target = ability.Caster.CurrentTarget.transform;
            var activeTime = Time.time + activeDelay;
            while (Time.time < activeTime)
            {
                rb2d.velocity = (target.position - transform.position).normalized * moveSpeed;
                yield return changeDirectionInterval.Wait();
            }
            indicator.SetActive(false);
            yield return Explosion(ability);
            ReturnToPool();
        }

        private IEnumerator Explosion(OrientationAbility ability)
        {
            explosion.SetActive(true);
            AudioManager.Play("IceCircleExplosion");
            var effectList = ListPool<IPoolObject>.Get();
            Fighter hitTarget = null;
            var hits = Physics2D.OverlapCircleAll(transform.position, damageRange, LayerMaskHelper.FigherMask);
            foreach (var hit in hits)
            {
                if (!(hit.TryGetComponent<Fighter>(out var fighter) && ability.IsRightTarget(fighter)))
                    continue;

                ability.HitThisFighter(fighter);
                fighter.Movement.Block(blockTime);
                hitTarget = fighter;
                if (PoolManager.Get<PoolObject>(ropePrefab, out var intance))
                {
                    intance.transform.position = fighter.Position;
                    intance.transform.SetParent(fighter.transform);
                    effectList.Add(intance);
                }
            }
            if (hitTarget != null && PoolManager.Get<ISpell>(minionSpawnSpellPrefab, out var spell))
            {
                spell.KickOff(ability, hitTarget.Position - ability.Caster.Owner.Position);
            }
            yield return blockTime.Wait();
            foreach (var effect in effectList)
            {
                effect.ReturnToPool();
            }
            ListPool<IPoolObject>.Release(effectList);

        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, damageRange);
        } 
#endif
    }
}