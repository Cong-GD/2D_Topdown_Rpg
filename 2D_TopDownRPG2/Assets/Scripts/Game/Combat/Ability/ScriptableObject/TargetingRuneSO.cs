using CongTDev.ObjectPooling;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CongTDev.AbilitySystem
{
    [CreateAssetMenu(fileName = "Targeting rune", menuName = "Rune/Targeting")]
    public class TargetingRuneSO : ActiveRuneSO
    {
        [field: SerializeField] public Prefab VfxPlayWhenActive { get; private set; }

        [SerializeField] private List<BaseEffectFactorySO> effectsApplyToTarget;
        public IEnumerable<BaseEffectFactorySO> EffectsApplyToTarget => effectsApplyToTarget;

        public override IAbility GetAbility()
        {
            return new TargetingAbility(this);
        }

        public override string GetDescription()
        {
            var description = new StringBuilder();
            description.AppendLine(this.Description);
            foreach (var effect in effectsApplyToTarget)
            {
                description.AppendLine(effect.EffectInfo.DesriptionWithColor);
            }
            return description.ToString();
        }

        private static readonly string[] _types = new string[] { "Rune", "Targeting" };

        public override IEnumerable<string> GetSubTypes() => _types;
    }
}