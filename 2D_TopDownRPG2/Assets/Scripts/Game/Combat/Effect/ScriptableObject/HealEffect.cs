using UnityEngine;

namespace CongTDev.AbilitySystem
{
    [CreateAssetMenu(fileName = "Recovery Effect", menuName = "Effects/Instant/Heal")]
    public class HealEffect : BaseEffectAndFactory
    {
        [SerializeField] protected StatBasedValue basedValue;

        public override void Instanciate(Fighter source, Fighter receiver)
        {
            float healAmount = basedValue.GetRawValue(source);
            receiver.Health.Recover(healAmount);
        }
    }
}