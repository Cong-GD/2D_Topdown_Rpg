using CongTDev.AbilitySystem.Spell;
using CongTDev.IOSystem;
using CongTDev.ObjectPooling;
using System;
using System.Text;

namespace CongTDev.AbilitySystem
{
    public class OrientationAbility : ActiveAbility<OrientationRune>
    {
        public OrientationAbility(OrientationRune rune)
            : base(rune)
        {
            AddSubType("Orientation");
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

            if (!IsEnoughMana())
            {
                return Respond.NotEnoughMana;
            }

            MarkAsJustUse();

            if (IsInstantCast())
            {
                StartSpell();
                return Respond.Success;
            }

            Caster.CastHelper(this, StartSpell);
            return Respond.InCasting;
        }

        private void StartSpell()
        {
            if (!IsEnoughMana())
                return;

            Caster.Owner.Mana.Draw(Rune.BaseManaConsume);

            if (PoolManager.Get<ISpell>(Rune.SpellReleaseWhenUse, out var spell))
            {
                spell.KickOff(this, Caster.LookDirection);
            }
        }

        public void HitThisFighter(Fighter target)
        {
            foreach (var builder in Rune.EffectsApplyToTarget)
            {
                target.ReceiveEffect(builder.Build(), Caster.Owner);
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
            return new SerializedOrientationAbility(this);
        }


        [Serializable]
        public class SerializedOrientationAbility : SerializedObject
        {
            public string runeJson;
            public SerializedOrientationAbility() { }
            public SerializedOrientationAbility(OrientationAbility ability)
            {
                runeJson = ability.Rune.ToWrappedJson();
            }

            public override object Deserialize()
            {
                var rune = (OrientationRune)JsonHelper.WrappedJsonToObject(runeJson);
                return new OrientationAbility(rune);
            }

            public override SerializedType GetSerializedType() => SerializedType.OrientationAbility;
        }
        #endregion
    }
}