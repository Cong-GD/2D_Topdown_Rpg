using CongTDev.IOSystem;
using CongTDev.ObjectPooling;
using System.Text;
using UnityEngine;

namespace CongTDev.AbilitySystem
{
    public class TargetingAbility : ActiveAbility<TargetingRune>
    {
        private Fighter _choosedTarget;

        public TargetingAbility(TargetingRune rune) : base(rune)
        {
            AddSubType("Targeting");
            AddSubTypeByEffects(rune.EffectsApplyToTarget);
        }

        public override void Install(AbilityCaster caster)
        {
            Caster = caster;
        }

        public override Respond TryUse()
        {
            if (!IsReady())
            {
                return Respond.InCooling;
            }

            if (Caster.IsCasting)
            {
                return Respond.AnotherAbilityInUse;
            }

            if (!IsRightTarget(Caster.CurrentTarget))
            {
                return Respond.InvalidTarget;
            }

            if (!IsEnoughMana())
            {
                return Respond.NotEnoughMana;
            }

            _choosedTarget = Caster.CurrentTarget;
            MarkAsJustUse();

            if (IsInstantCast())
            {
                Apply();
                return Respond.Success;
            }

            Caster.CastHelper(this, Apply);
            return Respond.InCasting;
        }

        private void Apply()
        {
            if (!IsEnoughMana())
                return;

            if (!IsRightTarget(_choosedTarget))
                return;

            Caster.Owner.Mana.Draw(Rune.BaseManaConsume);

            if (PoolManager.Get<IPoolObject>(Rune.VfxPlayWhenActive, out var vfx))
            {
                vfx.gameObject.transform.position = Caster.Owner.HitBox.bounds.center;
            }

            foreach (var builder in Rune.EffectsApplyToTarget)
            {
                _choosedTarget.ReceiveEffect(builder.Build(), Caster.Owner);
            }
        }

        public override string GetDescription()
        {
            var description = new StringBuilder();
            description.AppendLine($"Mana consume: {Rune.BaseManaConsume}");
            description.AppendLine($"Cast delay: {Rune.BaseCastDelay}");
            description.AppendLine($"Cooldown: {Rune.BaseCooldown}");
            description.AppendLine(Rune.Description);
            foreach (var effect in Rune.EffectsApplyToTarget)
            {
                description.AppendLine(effect.EffectInfo.DesriptionWithColor);
            }
            return description.ToString();
        }

        #region IOSystem
        public override SerializedObject Serialize()
        {
            return new SerializedTargetingAbility(this);
        }

        public class SerializedTargetingAbility : SerializedObject
        {
            public string runeJson;
            public SerializedTargetingAbility() { }

            public SerializedTargetingAbility(TargetingAbility ability)
            {
                runeJson = ability.Rune.ToWrappedJson();
            }
            public override object Deserialize()
            {
                var rune = (TargetingRune)JsonHelper.WrappedJsonToObject(runeJson);
                return new TargetingAbility(rune);
            }

            public override SerializedType GetSerializedType() => SerializedType.TargetingAbility;
        }
        #endregion
    }

}