using CongTDev.AbilitySystem;
using CongTDev.AbilitySystem.Spell;
using CongTDev.AudioManagement;
using CongTDev.ObjectPooling;
using System.Collections;
using UnityEngine;

public class BossStormSpell : PoolObject, ISpell
{
    private static readonly int stormHash = Animator.StringToHash("Storm");

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb2d;

    [SerializeField] private float startDelay;
    [SerializeField] private float lifeTime;
    [SerializeField] private float speed;
    [SerializeField] private float damageInterval;
    [SerializeField] private Vector2 damageZoneSize;

    private Fighter _target;
    private OrientationAbility _ability;
    private float _endTime;

    public void KickOff(OrientationAbility ability, Vector2 position)
    {
        _ability = ability;
        _target = ability.Caster.CurrentTarget;
        transform.position = position;
        StartCoroutine(StormCoroutine());
    }

    private IEnumerator StormCoroutine()
    {
        yield return startDelay.Wait();
        animator.Play(stormHash);
        _endTime = Time.time + lifeTime;
        StartCoroutine(DamageCoroutine());
        while(Time.time < _endTime)
        {
            AudioManager.Play("Storm").SetVolume(0.55f);
            var direction = (_target.Position - (Vector2)transform.position).normalized;
            rb2d.velocity = direction * speed;
            yield return 0.5f.Wait();
        }
        ReturnToPool();
    }

    private IEnumerator DamageCoroutine()
    {
        while(Time.time < _endTime)
        {
            var hits = Physics2D.OverlapBoxAll(transform.position, damageZoneSize, 0, LayerMaskHelper.FigherMask);
            foreach(var hit in hits)
            {
                if(hit.TryGetComponent<Fighter>(out var fighter) && _ability.IsRightTarget(fighter))
                {
                    _ability.HitThisFighter(fighter);
                }
            }
            yield return damageInterval.Wait();
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, damageZoneSize);
    } 
#endif
}
