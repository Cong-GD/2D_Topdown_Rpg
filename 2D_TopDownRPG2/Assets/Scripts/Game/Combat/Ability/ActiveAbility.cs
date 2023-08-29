using CongTDev.IOSystem;
using UnityEngine;

namespace CongTDev.AbilitySystem
{
    public abstract class ActiveAbility<T> : Ability<T>, IActiveAbility where T : ActiveRune
    {
        public float NextUseTime { get; protected set; } = Mathf.NegativeInfinity;
        public float CastDelay => Rune.BaseCastDelay;

        public float MaxUseRange => Rune.MaxUseRange;

        protected ActiveAbility(T rune) : base(rune)
        {
            AddSubType("Active Ability");
        }

        public abstract Respond TryUse();

        public abstract SerializedObject Serialize();

        public bool IsReady() => Time.time > NextUseTime;

        public bool IsEnoughMana() => Rune.BaseManaConsume <= Caster.Owner.Mana.Current;

        public bool IsInstantCast() => Rune.BaseCastDelay <= Mathf.Epsilon;

        public float GetCooldownTime()
            => Rune.BaseCooldown;

        public float CurrentCoolDown => Mathf.Max(0, NextUseTime - Time.time);

        public Fighter.Team GetTargetTeam()
        {
            if (Rune.TargetType == TargetType.Ally)
                return Caster.Owner.team;
            else
            {
                return Caster.Owner.team == Fighter.Team.Hero ? Fighter.Team.Monster : Fighter.Team.Hero;
            }
        }

        public bool IsRightTarget(Fighter target)
            => target != null && !target.IsInvisibility && GetTargetTeam() == target.team;

        protected void MarkAsJustUse()
        {
            NextUseTime = Time.time + GetCooldownTime();
        }
    }
}

