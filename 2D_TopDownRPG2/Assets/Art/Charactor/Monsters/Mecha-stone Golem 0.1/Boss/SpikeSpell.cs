using CongTDev.AudioManagement;
using CongTDev.ObjectPooling;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace CongTDev.AbilitySystem.Spell
{
    public class SpikeSpell : PoolObject, ISpell
    {
        [SerializeField] private Transform damageZone;
        [SerializeField] private Animator animator;
        [SerializeField] private Collider2D collider2d;
        [SerializeField] private Prefab explosionPrefab;

        [SerializeField] private float damageRadius;
        [SerializeField] private float explosionRadius;
        [SerializeField] private float zoneScale;
        [SerializeField] private float indicateDuration;
        [SerializeField] private float waitAfterIndicated;
        [SerializeField] private float timeOffsetAfterAnimation;
        [SerializeField] private float spikeLifeTime;

        private void OnDisable()
        {
            collider2d.enabled = false;
        }

        public void KickOff(OrientationAbility ability, Vector2 position)
        {
            transform.position = position;
            StartCoroutine(SpellCoroutine(ability));
        }

        private IEnumerator SpellCoroutine(OrientationAbility ability)
        {
            damageZone.localScale = Vector3.zero;
            damageZone.DOScale(Vector3.one * zoneScale, indicateDuration);
            yield return indicateDuration.Wait();
            yield return waitAfterIndicated.Wait();
            damageZone.localScale = Vector3.zero;
            animator.Play("SpikeUp");
            yield return timeOffsetAfterAnimation.Wait();
            AudioManager.Play("SpikeUp").SetVolume(0.4f);
            collider2d.enabled = true;
            DealRangeDamage(ability, damageRadius);
            yield return spikeLifeTime.Wait();
            AudioManager.Play("RockExplosion").SetVolume(0.4f);
            collider2d.enabled = false;
            yield return 0.1f.Wait();
            Explosion(ability);
            ReturnToPool();
        }

        private void DealRangeDamage(OrientationAbility ability, float radius)
        {
            var hits = Physics2D.OverlapCircleAll(damageZone.position, radius, LayerMaskHelper.FigherMask);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<Fighter>(out var fighter) && ability.IsRightTarget(fighter))
                {
                    ability.HitThisFighter(fighter);
                }
            }
        }

        private void Explosion(OrientationAbility ability)
        {
            if (PoolManager.Get<PoolObject>(explosionPrefab, out var intance))
            {
                intance.transform.position = damageZone.position;
                
                DealRangeDamage(ability, explosionRadius);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(damageZone.position, damageRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(damageZone.position, explosionRadius);
        }
#endif

    }
}

