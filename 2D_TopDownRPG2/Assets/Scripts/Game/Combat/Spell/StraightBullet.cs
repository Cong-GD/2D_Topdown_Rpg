using CongTDev.AudioManagement;
using CongTDev.ObjectPooling;
using System.Collections;
using UnityEngine;

namespace CongTDev.AbilitySystem.Spell
{
    public class StraightBullet : PoolObject, ISpell
    {
        [SerializeField] private Rigidbody2D rb2d;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float lifeTime;

        protected OrientationAbility ability;

        [Space]
        [Header("Sound")]
        [SerializeField] private string soundPlayWhenStart;
        [SerializeField] private string soundPlayWhenHit;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Fighter>(out var fighter) && ability.IsRightTarget(fighter))
            {
                OnHitTarget(fighter);
                if (!string.IsNullOrEmpty(soundPlayWhenHit))
                {
                    AudioManager.Play(soundPlayWhenHit);
                }
            }
        }

        protected virtual void OnHitTarget(Fighter fighter)
        {
            ability.HitThisFighter(fighter);
            ReturnToPool();
        }

        public virtual void KickOff(OrientationAbility ability, Vector2 direction)
        {
            this.ability = ability;

            transform.position = ability.Caster.Owner.HitBox.bounds.center;
            direction.Normalize();
            rb2d.velocity = direction * moveSpeed;
            var rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);

            if(!string.IsNullOrEmpty(soundPlayWhenStart))
            {
                AudioManager.Play(soundPlayWhenStart);
            }

            StartCoroutine(LifeCheckCoroutine());
        }

        protected virtual IEnumerator LifeCheckCoroutine()
        {
            float lifeTimeCheckInterval = 0.2f;
            Vector3 startPosition = transform.position;
            float endTime = Time.time + lifeTime;
            while (Time.time < endTime)
            {
                var distanceFromStartPosition = Vector3.Distance(transform.position, startPosition);
                if (distanceFromStartPosition > ability.MaxUseRange)
                {
                    break;
                }
                yield return lifeTimeCheckInterval.Wait();
            }
            ReturnToPool();
        }
    }
}