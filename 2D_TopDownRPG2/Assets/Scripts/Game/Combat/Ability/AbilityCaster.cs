using DG.Tweening;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace CongTDev.AbilitySystem
{
    public class AbilityCaster : MonoBehaviour
    {
        public List<IPassiveAbility> passiveAbilities = new();
        public event Action<IActiveAbility> OnCastingStart;
        public event Action<float> OnCastingProgress;
        public event Action OnCastingEnd;

        private Tweener _tweener;
        private Vector2 _lookDirection;

        [field: SerializeField] public Fighter Owner { get; private set; }
        public bool IsCasting { get; private set; }
        public Fighter CurrentTarget { get; set; }

        /// <summary>
        /// A vector with origin from caster. Haven't limit magnitude
        /// </summary>
        public Vector2 LookDirection
        {
            get => _lookDirection;
            set => _lookDirection = value - (Vector2)Owner.HitBox.bounds.center;
        }

        public void CastHelper(IActiveAbility ability, Action skillAction)
        {
            OnCastingStart?.Invoke(ability);
            IsCasting = true;

            _tweener = DOVirtual.Float(0, 1, ability.CastDelay, (progress) => OnCastingProgress?.Invoke(progress))
               .SetEase(Ease.Linear)
               .OnComplete(() => OnCastingCompleting(skillAction));
        }

        public void CollapseCasting()
        {
            if (!IsCasting)
                return;

            IsCasting = false;
            _tweener?.Kill();
            OnCastingEnd?.Invoke();
        }

        private void OnCastingCompleting(Action skillAction)
        {
            IsCasting = false;
            skillAction?.Invoke();
            OnCastingEnd?.Invoke();
        }

        public void AddPassiveAbility(IPassiveAbility passiveAbility)
        {
            passiveAbility.Install(this);
            passiveAbilities.Add(passiveAbility);
        }

        public void RemovePassiveAbility(IPassiveAbility passiveAbility)
        {
            if (passiveAbilities.Contains(passiveAbility))
            {
                passiveAbility.CleanUp();
                passiveAbilities.Remove(passiveAbility);
            }
        }
    }
}