using CongTDev.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CongTDev.AbilitySystem
{
    [CreateAssetMenu(fileName = "Self active rune", menuName = "Rune/Self active")]
    public class SelfActiveRune : ActiveRune
    {
        [field: SerializeField] public Prefab VfxWhenUse { get; private set; }

        [field: SerializeField] public string SFVWhenUse { get; private set; }

        [SerializeField] private List<BaseEffectFactory> effectsApplyToCaster;
        public IEnumerable<BaseEffectFactory> EffectsApplyToCaster => effectsApplyToCaster;

        public override IItem CreateItem()
        {
            return new SelfActiveAbility(this);
        }

        private static readonly string[] _types = new string[] { "Rune", "Self Active" };

        public override IEnumerable<string> GetSubTypes() => _types;
    }
}