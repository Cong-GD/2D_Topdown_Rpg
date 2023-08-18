using CongTDev.ObjectPooling;
using System.Collections;
using UnityEngine;

namespace CongTDev.AbilitySystem.Spell
{
    public class ZoneDamage : PoolObject, ISpell
    {
        private static readonly int damageZoneHash = Animator.StringToHash("StartedZoneDamage");

        [SerializeField] private Animator animator;
        [SerializeField] private float prepareTime;
        [SerializeField] private float duration;
        [SerializeField] private float damageTimeElapse;
        [SerializeField] private float damageRange;

        private OrientationAbility _ability;
        
        public void KickOff(OrientationAbility ability, Vector2 direction)
        {
            _ability = ability;
            direction = Vector2.ClampMagnitude(direction, ability.MaxUseRange);
            transform.position = ability.Caster.transform.position + (Vector3)direction;
            StartCoroutine(StartPrepare());
        }

        private IEnumerator StartPrepare()
        {
            yield return prepareTime.Wait();
            animator.Play(damageZoneHash);
            var endTime = Time.time + duration;
            while (Time.time < endTime)
            {
                var hits = Physics2D.OverlapCircleAll(transform.position, damageRange, LayerMaskHelper.FigherMask);
                foreach (var hit in hits)
                {
                    if(hit.TryGetComponent<Fighter>(out var fighter) && _ability.IsRightTarget(fighter))
                    {
                        _ability.HitThisFighter(fighter);
                    }
                }
                yield return damageTimeElapse.Wait();
            }
            ReturnToPool();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, damageRange);
        }
#endif
    }
}

