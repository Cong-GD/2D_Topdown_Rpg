using CongTDev.ObjectPooling;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CongTDev.AbilitySystem
{
    [CreateAssetMenu(fileName = "Orientation rune", menuName = "Rune/Orientation")]
    public class OrientationRune : ActiveRune
    {
        private static readonly string[] _types = new string[] { "Rune", "Orientation" };

        [SerializeField] private List<BaseEffectFactory> effectsApplyToTarget;

        [field: SerializeField] public Prefab SpellReleaseWhenUse { get; private set; }

        public IReadOnlyCollection<BaseEffectFactory> EffectsApplyToTarget => effectsApplyToTarget;

        public override IItem CreateItem()
        {
            return new OrientationAbility(this);
        }

        public override IEnumerable<string> GetSubTypes() => _types;
    }
}