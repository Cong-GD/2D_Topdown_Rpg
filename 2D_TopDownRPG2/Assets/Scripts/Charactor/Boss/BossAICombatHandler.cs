using CongTDev.AbilitySystem;
using CongTDev.AudioManagement;
using CongTDev.ObjectPooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CongTDev.TheBoss
{
    public class BossAICombatHandler : MonoBehaviour
    {
        private readonly int idleHash = Animator.StringToHash("Idle");

        [Header("BasicReference")]
        [SerializeField] private BaseStatData bossStats;
        [SerializeField] private Animator animator;
        [SerializeField] private AbilityCaster abilityCaster;
        [SerializeField] private float updateInterval;

        [Header("Abilities")]
        [SerializeField] private List<AbilityHandler> abilityHandlers;
        [SerializeField] private List<AbilityWorkFlowNode> workFlowState1;
        [SerializeField] private List<AbilityWorkFlowNode> workFlowState2;

        [Serializable]
        public class AbilityHandler
        {
            public ActiveRune rune;
            public float prefixWait;
            public float suffixWait;
            public string animationName;
            [HideInInspector]
            public IActiveAbility ability;
            [HideInInspector]
            public bool hasAnimation;
            [HideInInspector]
            public int animationHash;
        }

        [Serializable]
        public class AbilityWorkFlowNode
        {
            public AbilityName abilityToUse;
            public float cooldown;
        }

        public enum AbilityName
        {
            HandFire,
            SpikeFire,
            LazerFire,
            StormSpell,
            SpawnMonster,
            DamageZone
        }

        private Fighter _playerCombat;
        private float _allowAbilityUseTime;
        private int _currentNode;

        private bool IsDead() => abilityCaster.Owner.Health.IsEmpty;


        public IEnumerator StartCombatState()
        {
            yield return PreStartCombat();
            yield return State1();
            yield return State2();
            yield return EndCombat();
        }

        private IEnumerator PreStartCombat()
        {
            abilityCaster.Owner.Stats.SetStatBase(bossStats);
            var levelModifiers = bossStats.growStat.GetGrowingStat(PlayerLevelSystem.CurrentLevel);
            foreach( var modifier in levelModifiers )
            {
                abilityCaster.Owner.Stats.ApplyModifier(modifier.Key, modifier.Value);
            }
            animator.Play("Unimmune");
            while (!abilityCaster.Owner.Health.IsFull)
            {
                abilityCaster.Owner.Health.RecorverByRatio(0.02f);
                yield return CoroutineHelper.fixedUpdateWait;
            }
            InitAbilities();
            yield return 1f.Wait();
        }

        private IEnumerator State1()
        {
            _allowAbilityUseTime = Time.time;
            _currentNode = 0;
            while (abilityCaster.Owner.Health.Ratio > 0.5f)
            {
                yield return AbilityUseCoroutine(workFlowState1);
                yield return updateInterval.Wait();
            }
        }
        private IEnumerator State2()
        {
            var defenceBuff = new StatModifier(100, StatModifier.BonusType.Flat);
            abilityCaster.Owner.Stats.ApplyModifier(Stat.Defence, defenceBuff);
            _allowAbilityUseTime = Time.time;
            _currentNode = 0;
            while (!IsDead())
            {
                yield return AbilityUseCoroutine(workFlowState2);
                yield return updateInterval.Wait();
            }
        }

        private IEnumerator EndCombat()
        {
            PoolManager.ClearPool();
            AudioManager.Play("BossDeath");
            yield return 1.5f.Wait();
            animator.Play("Death");
        }

        private void InitAbilities()
        {
            _playerCombat = PlayerController.Instance.Combat;
            abilityCaster.CurrentTarget = _playerCombat;
            foreach (var handler in abilityHandlers)
            {
                handler.animationHash = Animator.StringToHash(handler.animationName);
                handler.hasAnimation = animator.HasState(0, handler.animationHash);
                handler.ability = (IActiveAbility)handler.rune.CreateItem();
                handler.ability.Install(abilityCaster);
            }
        }
        private IEnumerator AbilityUseCoroutine(List<AbilityWorkFlowNode> workFlowState)
        {
            if (Time.time > _allowAbilityUseTime)
            {
                yield return UseAbility(abilityHandlers[(int)workFlowState[_currentNode].abilityToUse]);
                if(IsDead())
                {
                    yield break;
                }
                _allowAbilityUseTime = Time.time + workFlowState[_currentNode].cooldown;
                _currentNode++;
                _currentNode %= workFlowState.Count;
                animator.Play(idleHash);
            }
            
        }

        private IEnumerator UseAbility(AbilityHandler handler)
        {
            if(handler.hasAnimation)
            {
                animator.Play(handler.animationHash);
            }
            yield return handler.prefixWait.Wait();
            if (IsDead())
            {
                yield break;
            }
            abilityCaster.LookDirection = _playerCombat.Position;
            handler.ability.TryUse();
            yield return handler.suffixWait.Wait();
        }
    }
}