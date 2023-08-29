using System;
using System.Collections.Generic;
using UnityEngine;

namespace CongTDev.AbilitySystem
{
    public abstract class Ability<T> where T : Rune
    {
        public readonly T Rune;
        public event Action OnDestroy;

        private readonly List<string> _subTypes = new();

        public AbilityCaster Caster { get; protected set; }
        public Sprite Icon => Rune.AbilityIcon;
        public ItemRarity Rarity => Rune.Rarity;
        public string Name => Rune.Name;
        public string ItemType => IAbility.ITEM_TYPE;

        public Ability(T rune)
        {
            Rune = rune;
        }

        public IEnumerable<string> GetSubTypes() => _subTypes;

        public virtual void Destroy()
        {
            OnDestroy?.Invoke();
        }

        public abstract void Install(AbilityCaster caster);

        public abstract string GetDescription();

        protected void AddSubType(string type)
        {
            if (!string.IsNullOrEmpty(type) && !_subTypes.Contains(type))
            {
                _subTypes.Add(type);
            }
        }

        protected void AddSubTypeByEffects(IEnumerable<BaseEffectFactory> effectBuiders)
        {
            foreach (var effectBuider in effectBuiders)
            {
                AddSubType(effectBuider.EffectInfo.EffectTypeInfo);
            }
        }
    }
}

