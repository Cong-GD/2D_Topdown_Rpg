using CongTDev.AbilitySystem;
using System.Collections;
using UnityEngine;

public class RangeAICombatBehaviour : BaseAICombatBehaviour
{
    [Header("Range control fields")]
    [SerializeField] protected ActiveRune activeRuneSO;
    [SerializeField] protected float movingRange;
    [SerializeField] protected float movingAngle;
    [SerializeField] protected float aimTime;
    [SerializeField] protected float combatExitTime;

    [Header("Secondary ability")]
    [SerializeField] private OrientationRune damageZoneRune;
    [SerializeField] private float secondAbilityStartUseTime;
    [Min(0.1f)]
    [SerializeField] protected float secondAbilityCheckInterval;

    private IActiveAbility _ability;
    private IActiveAbility _damageZoneAbility;

    public override void Prepare(MonstersAI monstersAI)
    {
        base.Prepare(monstersAI);
        _ability = (IActiveAbility)activeRuneSO.CreateItem();
        _ability.Install(abilityCaster);
        _damageZoneAbility = (IActiveAbility)damageZoneRune.CreateItem();
        _damageZoneAbility.Install(abilityCaster);
    }

    protected override IEnumerator CombatHandler()
    {
        var secondAbilityRoutine = StartCoroutine(SecondAbilityCoroutine());
        var exitTime = Time.time + combatExitTime;
        while (IsAlive())
        {
            var distanceToPlayer = Vector2.Distance(transform.position, monsterAI.PlayerPosition);
            var distanceToStartPosition = Vector2.Distance(transform.position, monsterAI.StartPosition);
            if (Time.time > exitTime && (distanceToPlayer > vision || distanceToStartPosition > maxMoveRange))
            {
                if (secondAbilityRoutine != null)
                {
                    StopCoroutine(secondAbilityRoutine);
                }
                yield break;
            }
            if (distanceToPlayer < attackRange)
            {

                switch (_ability.TryUse())
                {
                    case Respond.Success:
                        abilityCaster.LookDirection = monsterAI.PlayerPosition;
                        abilityCaster.Owner.Movement.Block(0.3f);
                        break;
                    case Respond.InCasting:
                        StartCoroutine(AimCoroutine(_ability.CastDelay));
                        continue;
                }
            }
            monsterAI.MoveTo(GetRandomPositionFromAttackRange());
            yield return COMMON_UPDATE_INTERVAL.Wait();
        }
    }

    private Vector2 GetRandomPositionFromAttackRange()
    {
        var direction = ((Vector2)transform.position - monsterAI.PlayerPosition).normalized;
        var rotateRad = Random.Range(-movingAngle / 2, movingAngle / 2) * Mathf.Deg2Rad;
        var newRad = rotateRad + Mathf.Atan2(direction.y, direction.x);
        direction = new Vector2(Mathf.Cos(newRad), Mathf.Sin(newRad));
        return monsterAI.PlayerPosition + direction * movingRange;
    }

    private IEnumerator AimCoroutine(float castDelay)
    {
        yield return (castDelay - aimTime).Wait();
        abilityCaster.LookDirection = monsterAI.PlayerPosition;
    }

    private IEnumerator SecondAbilityCoroutine()
    {
        yield return secondAbilityStartUseTime.Wait();
        while (IsAlive())
        {
            if (!_damageZoneAbility.IsReady())
            {
                yield return secondAbilityCheckInterval.Wait();
                continue;
            }

            var distanceToPlayer = Vector2.Distance(transform.position, monsterAI.PlayerPosition);
            if (distanceToPlayer > _damageZoneAbility.MaxUseRange)
            {
                yield return secondAbilityCheckInterval.Wait();
                continue;
            }

            abilityCaster.LookDirection = monsterAI.PlayerPosition;
            switch (_damageZoneAbility.TryUse())
            {
                case Respond.Success:
                    yield return _damageZoneAbility.CurrentCoolDown.Wait();
                    break;
                case Respond.InCasting:
                    yield return AimCoroutine(_damageZoneAbility.CastDelay);
                    yield return _damageZoneAbility.CurrentCoolDown.Wait();
                    break;
            }
            yield return secondAbilityCheckInterval.Wait();
        }
    }
}
