using CongTDev.AbilitySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CongTDev.TheBoss
{
    public class BossAICombatHandler : MonoBehaviour
    {
        private static readonly int idleHash = Animator.StringToHash("Idle");

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
            public ActiveRuneSO rune;
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
            public float cooldown;
            public int connect;
        }

        [Header("Event")]
        public UnityEvent OnStartCombat;
        public UnityEvent OnEndCombat;

        private Fighter _playerCombat;
        private float _allowAbilityUseTime;
        private int _currentAbility;

        private void OnValidate()
        {
            while (workFlowState1.Count < abilityHandlers.Count)
            {
                workFlowState1.Add(new());
            }
            while (workFlowState2.Count < abilityHandlers.Count)
            {
                workFlowState2.Add(new());
            }
        }
        private void Awake()
        {
            workFlowState1.ForEach(node => node.connect = Mathf.Clamp(node.connect, 0, abilityHandlers.Count));
            workFlowState2.ForEach(node => node.connect = Mathf.Clamp(node.connect, 0, abilityHandlers.Count));
        }

        public IEnumerator StartCombatState()
        {
            yield return PreStartCombat();
            yield return State1();
            yield return State2();
            yield return EndCombat();
        }

        private IEnumerator PreStartCombat()
        {
            OnStartCombat.Invoke();
            abilityCaster.Owner.Stats.SetStatBase(bossStats);
            while (!abilityCaster.Owner.Health.IsFull)
            {
                abilityCaster.Owner.Health.RecorverByRatio(0.05f);
                yield return CoroutineHelper.fixedUpdateWait;
            }
            InitAbilities();
            yield break;
        }

        private IEnumerator State1()
        {
            _allowAbilityUseTime = Time.time;
            _currentAbility = 0;
            while (abilityCaster.Owner.Health.Ratio > 0.5f)
            {
                yield return AbilityUseCoroutine(workFlowState1);
                yield return updateInterval.Wait();
            }
        }
        private IEnumerator State2()
        {
            _allowAbilityUseTime = Time.time;
            _currentAbility = 0;
            while (!abilityCaster.Owner.Health.IsEmpty)
            {
                yield return AbilityUseCoroutine(workFlowState2);
                yield return updateInterval.Wait();
            }
        }

        private IEnumerator EndCombat()
        {
            animator.Play("Death");
            yield return 2f.Wait();
            yield break;
        }

        private void InitAbilities()
        {
            _playerCombat = PlayerController.Instance.Combat;
            abilityCaster.CurrentTarget = _playerCombat;
            foreach (var handler in abilityHandlers)
            {
                handler.animationHash = Animator.StringToHash(handler.animationName);
                handler.hasAnimation = animator.HasState(0, handler.animationHash);
                handler.ability = (IActiveAbility)handler.rune.GetAbility();
                handler.ability.Install(abilityCaster);
            }
        }
        private IEnumerator AbilityUseCoroutine(List<AbilityWorkFlowNode> workFlowState)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && Time.time > _allowAbilityUseTime)
            {
                yield return UseAbility(abilityHandlers[_currentAbility]);
                if(abilityCaster.Owner.Health.IsEmpty)
                {
                    yield break;
                }
                _allowAbilityUseTime = Time.time + workFlowState[_currentAbility].cooldown;
                _currentAbility = workFlowState[_currentAbility].connect;
                animator.Play(idleHash);
            }
            yield break;
        }

        private IEnumerator UseAbility(AbilityHandler handler)
        {
            if(handler.hasAnimation)
            {
                animator.Play(handler.animationHash);
            }
            yield return handler.prefixWait.Wait();
            if (abilityCaster.Owner.Health.IsEmpty)
            {
                yield break;
            }
            abilityCaster.LookDirection = _playerCombat.Position;
            handler.ability.TryUse();
            yield return handler.suffixWait.Wait();
        }
    }
}