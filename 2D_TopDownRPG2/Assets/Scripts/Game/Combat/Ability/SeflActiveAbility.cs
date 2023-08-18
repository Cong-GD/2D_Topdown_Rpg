using CongTDev.IOSystem;
using CongTDev.ObjectPooling;
using UnityEngine;

namespace CongTDev.AbilitySystem
{
    public class SeflActiveAbility : ActiveAbility<SelfActiveRuneSO>
    {
        public SeflActiveAbility(SelfActiveRuneSO rune) : base(rune)
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

            foreach (var builder in Rune.EffectsApplyToCaster)
            {
                Caster.Owner.ReceiveEffect(builder.Build(), Caster.Owner);
            }
        }

        public override string GetDescription()
        {
            return Rune.GetDescription();
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

            public SerializedSeflActiveAbility(SeflActiveAbility ability)
            {
                runeJson = ability.Rune.ToWrappedJson();
            }

            public override object Deserialize()
            {
                var rune = (SelfActiveRuneSO)JsonHelper.WrappedJsonToObject(runeJson);
                return new SeflActiveAbility(rune);
            }

            public override SerializedType GetSerializedType() => SerializedType.SeflActiveAbility;
        }

        #endregion

    }

}