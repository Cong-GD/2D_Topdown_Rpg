using CongTDev.AudioManagement;
using CongTDev.IOSystem;
using CongTDev.ObjectPooling;
using System.Text;

namespace CongTDev.AbilitySystem
{
    public class SelfActiveAbility : ActiveAbility<SelfActiveRune>
    {
        public SelfActiveAbility(SelfActiveRune rune) : base(rune)
        {
            AddSubType("Sefl Active");
            AddSubTypeByEffects(rune.EffectsApplyToCaster);
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

            Caster.Owner.Mana.Draw(Rune.BaseManaConsume);

            if (PoolManager.Get<IPoolObject>(Rune.VfxWhenUse, out var vfx))
            {
                vfx.gameObject.transform.position = Caster.Owner.HitBox.bounds.center;
            }

            if (!string.IsNullOrEmpty(Rune.SFVWhenUse))
            {
                AudioManager.Play(Rune.SFVWhenUse);
            }

            foreach (var builder in Rune.EffectsApplyToCaster)
            {
                Caster.Owner.ReceiveEffect(builder.Build(), Caster.Owner);
            }
        }

        public override string GetDescription()
        {
            var description = new StringBuilder();
            description.AppendLine($"Mana consume: {Rune.BaseManaConsume}");
            description.AppendLine($"Cast delay: {Rune.BaseCastDelay}");
            description.AppendLine($"Cooldown: {Rune.BaseCooldown}");
            description.AppendLine(Rune.Description);
            foreach (var effect in Rune.EffectsApplyToCaster)
            {
                description.AppendLine(effect.EffectInfo.DesriptionWithColor);
            }
            return description.ToString();
        }

        #region IOSystem
        public override SerializedObject Serialize()
        {
            return new SerializedSeflActiveAbility(this);
        }

        public class SerializedSeflActiveAbility : SerializedObject
        {
            public string runeJson;

            public SerializedSeflActiveAbility() { }

            public SerializedSeflActiveAbility(SelfActiveAbility ability)
            {
                runeJson = ability.Rune.ToWrappedJson();
            }

            public override object Deserialize()
            {
                var rune = (SelfActiveRune)JsonHelper.WrappedJsonToObject(runeJson);
                return new SelfActiveAbility(rune);
            }

            public override SerializedType GetSerializedType() => SerializedType.SeflActiveAbility;
        }

        #endregion

    }

}