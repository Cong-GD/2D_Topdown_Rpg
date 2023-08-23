using CongTDev.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CongTDev.AbilitySystem
{
    [CreateAssetMenu(fileName = "Self active rune", menuName = "Rune/Self active")]
    public class SelfActiveRuneSO : ActiveRuneSO
    {
        [field: SerializeField] public Prefab VfxWhenUse { get; private set; }

        [SerializeField] private List<BaseEffectFactory> effectsApplyToCaster;
        public IEnumerable<BaseEffectFactory> EffectsApplyToCaster => effectsApplyToCaster;

        public override IItem CreateItem()
        {
            return new SeflActiveAbility(this);
        }

        public override string GetDescription()
        {
            var description = new StringBuilder();
            description.AppendLine(this.Description);
            foreach (var effect in effectsApplyToCaster)
            {
                description.AppendLine(effect.EffectInfo.DesriptionWithColor);
            }
            return description.ToString();
        }

        private static readonly string[] _types = new string[] { "Rune", "Self Active" };

        public override IEnumerable<string> GetSubTypes() => _types;
    }
}