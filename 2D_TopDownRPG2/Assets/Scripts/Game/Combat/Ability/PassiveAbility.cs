using CongTDev.IOSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace CongTDev.AbilitySystem
{
    public class PassiveAbility : Ability<PassiveRune>, IPassiveAbility
    {
        private List<IEffect> _appliedEffect;
        private bool _isInstalled;

        public PassiveAbility(PassiveRune rune) : base(rune)
        {
            AddSubType("Passive Ability");
            AddSubTypeByEffects(rune.EffectsApplyWhenEquip);
        }

        public override string GetDescription()
        {
            var description = new StringBuilder();
            description.AppendLine(Rune.Description);
            foreach (var effect in Rune.EffectsApplyWhenEquip)
            {
                description.AppendLine(effect.EffectInfo.DesriptionWithColor);
            }
            return description.ToString();
        }

        public override void Install(AbilityCaster caster)
        {
            if (_isInstalled) return;
            _isInstalled = true;
            Caster = caster;

            _appliedEffect = new List<IEffect>();

            foreach (var builder in Rune.EffectsApplyWhenEquip)
            {
                IEffect effect = builder.Build();
                caster.Owner.ReceiveEffect(effect, caster.Owner);
                _appliedEffect.Add(effect);
            }
        }
        public void CleanUp()
        {
            if (!_isInstalled) return;

            _isInstalled = false;
            foreach (IEffect effect in _appliedEffect)
            {
                Caster.Owner.RemoveEffect(effect);
            }
        }

        #region IOSystem

        public SerializedObject Serialize()
        {
            return new PassiveAbilitySerialize(this);
        }

        [Serializable]
        public class PassiveAbilitySerialize : SerializedObject
        {
            public string runeJson;

            public PassiveAbilitySerialize() { }
            public PassiveAbilitySerialize(PassiveAbility passiveAbility)
            {
                runeJson = passiveAbility.Rune.ToWrappedJson();
            }

            public override object Deserialize()
            {
                PassiveRune rune = (PassiveRune)JsonHelper.WrappedJsonToObject(runeJson);
                return new PassiveAbility(rune);
            }

            public override SerializedType GetSerializedType() => SerializedType.PassiveAbility;
        }
        #endregion
    }
}


