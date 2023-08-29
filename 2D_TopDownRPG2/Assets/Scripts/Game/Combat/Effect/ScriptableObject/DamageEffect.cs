using UnityEngine;
using UnityEngine.Pool;

namespace CongTDev.AbilitySystem
{
    [CreateAssetMenu(fileName = "Damage", menuName = "Effects/Instant/Damage")]
    public class DamageEffect : BaseEffectAndFactory
    {
        [SerializeField] protected DamageType damageType;
        [SerializeField] protected StatBasedValue damageBased;

        public override void Instanciate(Fighter source, Fighter target)
        {
            float rawDamage = damageBased.GetRawValue(source);
            var damageBlock = GenericPool<DamageBlock>.Get();
            damageBlock.Init(damageType, source, rawDamage);
            target.TakeDamage(damageBlock);
            GenericPool<DamageBlock>.Release(damageBlock);
        }
    }
}