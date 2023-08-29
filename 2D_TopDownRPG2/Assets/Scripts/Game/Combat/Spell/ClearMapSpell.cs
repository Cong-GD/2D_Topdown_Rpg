using CongTDev.AudioManagement;
using CongTDev.ObjectPooling;
using System.Collections;
using UnityEngine;

namespace CongTDev.AbilitySystem.Spell
{
    public class ClearMapSpell : PoolObject, ISpell
    {
        [SerializeField] private Prefab hitPrefab;
        [SerializeField] private float radius;
        [SerializeField] private float duration;
        [SerializeField] private Vector2 offset;
        [SerializeField] private OrientationAbility _ability;

        public void KickOff(OrientationAbility ability, Vector2 _)
        {
            _ability = ability;
            transform.position = ability.Caster.Owner.Position + offset;
            transform.SetParent(ability.Caster.transform);
            StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {
            var endTime = Time.time + duration;
            while(Time.time < endTime)
            {
                var hits = Physics2D.OverlapCircleAll(transform.position, radius, LayerMaskHelper.FigherMask);
                foreach (var hit in hits)
                {
                    if (hit.TryGetComponent<Fighter>(out var fighter) && _ability.IsRightTarget(fighter))
                    {
                        if (PoolManager.Get<PoolObject>(hitPrefab, out var instance))
                        {
                            instance.transform.position = fighter.Position;
                            AudioManager.Play("Thunder");
                        }
                        _ability.HitThisFighter(fighter);
                        yield return 0.1f.Wait();
                        if (Time.time > endTime || _ability == null)
                            break;
                    }
                }
                yield return 0.5f.Wait();
            }
            ReturnToPool();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
#endif
    }

}

