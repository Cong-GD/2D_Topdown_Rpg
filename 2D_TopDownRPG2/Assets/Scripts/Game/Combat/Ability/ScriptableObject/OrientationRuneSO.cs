using CongTDev.ObjectPooling;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CongTDev.AbilitySystem
{
    [CreateAssetMenu(fileName = "Orientation rune", menuName = "Rune/Orientation")]
    public class OrientationRuneSO : ActiveRuneSO
    {
        private static readonly string[] _types = new string[] { "Rune", "Orientation" };

        [SerializeField] private List<BaseEffectFactorySO> effectsApplyToTarget;

        [field: SerializeField] public Prefab SpellReleaseWhenUse { get; private set; }

        public IReadOnlyCollection<BaseEffectFactorySO> EffectsApplyToTarget => effectsApplyToTarget;

        public override IAbility GetAbility()
        {
            return new OrientationAbility(this);
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

        public override IEnumerable<string> GetSubTypes() => _types;
    }
}