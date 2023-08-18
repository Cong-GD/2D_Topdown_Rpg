﻿using CongTDev.AbilitySystem;
using System.Collections;
using UnityEngine;

public class MeleeAICombatBehaviour : BaseAICombatBehaviour
{
    [Header("Meele AI fields")]
    [SerializeField] protected ActiveRuneSO firstRune;
    [SerializeField] protected float movingRange;

    [Header("Secondary Ability")]
    [SerializeField] private ActiveRuneSO secondaryRune;
    [SerializeField] private float healThreshHold;
    [SerializeField] private float abilityCheckInterval;

    private IActiveAbility _ability;
    private IActiveAbility _secondaryAbility;

    public override void Prepare(MonstersAI monstersAI)
    {
        base.Prepare(monstersAI);
        _ability = (IActiveAbility)firstRune.GetAbility();
        _ability.Install(abilityCaster);
        _secondaryAbility = (IActiveAbility)secondaryRune.GetAbility();
        _secondaryAbility.Install(abilityCaster);
    }

    protected override IEnumerator CombatHandler()
    {
        var secondaryAbilityRoutine = StartCoroutine(SecondaryAbilityHandler());
        while (IsAlive())
        {
            var distanceToPlayer = Vector2.Distance(transform.position, monsterAI.PlayerPosition);
            var distanceToStartPosition = Vector2.Distance(transform.position, monsterAI.StartPosition);
            if (distanceToPlayer > vision || distanceToStartPosition > maxMoveRange)
            {
                StopCoroutine(secondaryAbilityRoutine);
                yield break;
            }
            if (distanceToPlayer < attackRange && _ability.IsReady())
            {
                abilityCaster.LookDirection = monsterAI.PlayerPosition;
                monsterAI.LookToward(monsterAI.PlayerPosition);
                var respond = _ability.TryUse();
                if (respond == Respond.Success || respond == Respond.InCasting)
                {
                    monsterAI.StopMove();
                    abilityCaster.Owner.Movement.Block(0.4f);
                    yield return COMMON_UPDATE_INTERVAL.Wait();
                    continue;
                }
            }
            monsterAI.MoveTo(monsterAI.PlayerPosition + Random.insideUnitCircle * movingRange);
            yield return COMMON_UPDATE_INTERVAL.Wait();
        }
    }

    private IEnumerator SecondaryAbilityHandler()
    {
        while (IsAlive())
        {
            if(abilityCaster.Owner.Health.Ratio < healThreshHold)
            {
                switch (_secondaryAbility.TryUse())
                {
                    case Respond.Success:
                        yield return _secondaryAbility.CurrentCoolDown.Wait();
                        break;
                    case Respond.InCasting:
                        abilityCaster.Owner.Movement.Block(_secondaryAbility.CastDelay);
                        yield return _secondaryAbility.CurrentCoolDown.Wait();
                        break;
                }
            }
            yield return abilityCheckInterval.Wait();
        }
    }
}