using UnityEngine;

namespace CongTDev.AbilitySystem
{
    [System.Serializable]
    public class EffectInfo
    {
        public enum EffectType
        {
            PhysicsDamage,
            MagicDamage,
            Buff,
            Debuff,
            Heal,
            DamageOverTime
        }

        private static string TypeToString(EffectType type)
            => type switch
            {
                EffectType.PhysicsDamage => "Physics damage",
                EffectType.MagicDamage => "Magic damage",
                EffectType.DamageOverTime => "Damage over time",
                _ => type.ToString()
            };

        [field: SerializeField] public EffectType EffectTypeSelect { get; private set; }

        [field: TextArea]
        [field: Tooltip("Decript effect info. Write the value it apply if needed")]
        [field: SerializeField] public string Description { get; private set; }

        [SerializeField] private Color _descriptionColor;

        public string DesriptionWithColor => Description.RichText(_descriptionColor);
        public string EffectTypeInfo => TypeToString(EffectTypeSelect);
    }

}