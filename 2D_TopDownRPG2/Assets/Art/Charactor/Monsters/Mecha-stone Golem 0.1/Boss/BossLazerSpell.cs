using CongTDev.AbilitySystem;
using CongTDev.AbilitySystem.Spell;
using CongTDev.AudioManagement;
using CongTDev.ObjectPooling;
using System.Collections;
using UnityEngine;

public class BossLazerSpell : PoolObject, ISpell
{
    [SerializeField] private float startSpellOffset = 1f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float damageInterval = 0.4f;
    [SerializeField] private float rotateDelta = 0.1f;
    [SerializeField] private Behaviour behaviour = Behaviour.RotateAround;

    [SerializeField] private Collider2D collider2d;

    private OrientationAbility _ability;
    private Fighter _target;

    public enum Behaviour
    {
        RotateAround,
        ChasingTarget
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Fighter>(out var fighter) && _ability.IsRightTarget(fighter))
        {
            _ability.HitThisFighter(fighter);
        }
    }

    public void KickOff(OrientationAbility ability, Vector2 direction)
    {
        var startPosition = ability.Caster.transform.parent.Find("LazePosition");
        if (startPosition != null)
        {
            transform.position = startPosition.position;
            transform.SetParent(startPosition);
        }
        else
        {
            transform.position = ability.Caster.Owner.Position;
            transform.SetParent(ability.Caster.transform);
        }
        transform.rotation = Quaternion.identity;
        _ability = ability;
        _target = ability.Caster.CurrentTarget;
        RotateToDirection(direction);
        StartCoroutine(LazerFiring());
    }

    public void SetBehaviour(Behaviour behaviour)
    {
        this.behaviour = behaviour;
    }

    private void RotateToDirection(Vector2 direction)
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }

    private IEnumerator LazerFiring()
    {
        collider2d.enabled = false;
        yield return startSpellOffset.Wait();
        RotateBehaviour();
        var endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            AudioManager.Play("LazerFiring");
            collider2d.enabled = true;
            yield return damageInterval.Wait();
            collider2d.enabled = false;
        }
        ReturnToPool();
    }

    private void RotateBehaviour()
    {
        switch (behaviour)
        {
            case Behaviour.RotateAround:
                StartCoroutine(OneRoundRotate());
                break;
            case Behaviour.ChasingTarget:
                StartCoroutine(ChasingPlayer());
                break;
        }
    }

    private IEnumerator OneRoundRotate()
    {
        while (true)
        {
            var targetAngle = transform.rotation.eulerAngles.z + rotateDelta;
            transform.rotation = Quaternion.Euler(0, 0, targetAngle);
            yield return CoroutineHelper.fixedUpdateWait;
        }
    }

    private IEnumerator ChasingPlayer()
    {
        while (true)
        {
            var targetDirection = _target.Position - (Vector2)transform.position;
            var targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            targetAngle = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, targetAngle, rotateDelta);
            transform.rotation = Quaternion.Euler(0, 0, targetAngle);
            yield return CoroutineHelper.fixedUpdateWait;
        }
    }

}
