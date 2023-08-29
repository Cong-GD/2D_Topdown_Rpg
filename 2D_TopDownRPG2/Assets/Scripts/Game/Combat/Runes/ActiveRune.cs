using UnityEngine;

namespace CongTDev.AbilitySystem
{
    public abstract class ActiveRune : Rune
    {
        [field: SerializeField] public TargetType TargetType { get; private set; }
        [field: SerializeField] public float BaseCastDelay { get; private set; }
        [field: SerializeField] public float BaseManaConsume { get; private set; }
        [field: SerializeField] public float BaseCooldown { get; private set; }
        [field: SerializeField] public float MaxUseRange { get; private set; }
    }
}