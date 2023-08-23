using System;
using UnityEngine;

namespace CongTDev.AbilitySystem
{
    [CreateAssetMenu(fileName = "BuffDeBuff", menuName = "Effects/Permanent/BuffDeBuff")]
    public class BuffEffectFactory : BaseEffectFactory
    {
        [SerializeField] protected Stat targetType;
        [SerializeField] protected float modifierValue;
        [SerializeField] protected StatModifier.BonusType modifierType;
        [SerializeField] protected bool customOrder;
        [Tooltip("Order in determine this modifier will apply before or after other modifier")]
        [SerializeField] protected int order;

        public override IEffect Build()
        {
            return new BuffEffect(this);
        }

        public class BuffEffect : IEffect
        {
            private readonly BuffEffectFactory _effectFactory;
            private StatModifier _appliedModifier;
            private Fighter _appliedTarget;

            public BuffEffect(BuffEffectFactory effectFactory)
            {
                _effectFactory = effectFactory;
            }

            public EffectInfo EffectInfo => _effectFactory.EffectInfo;
            public virtual void Instanciate(Fighter source, Fighter target)
            {
                var modifierOrder = _effectFactory.customOrder ? _effectFactory.order : (int)_effectFactory.modifierType;
                _appliedModifier = new StatModifier(_effectFactory.modifierValue, _effectFactory.modifierType, modifierOrder);

                target.Stats.ApplyModifier(_effectFactory.targetType, _appliedModifier);
                _appliedTarget = target;
            }


            public virtual void CleanUp()
            {
                _appliedTarget.Stats.RemoveModifier(_effectFactory.targetType, _appliedModifier);
            }
        }
    }
}