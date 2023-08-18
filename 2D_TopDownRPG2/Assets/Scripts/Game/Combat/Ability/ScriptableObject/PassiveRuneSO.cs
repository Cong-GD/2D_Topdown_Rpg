using System.Collections.Generic;
using System.Text;
using UnityEngine.Pool;
using UnityEngine;

namespace CongTDev.AbilitySystem
{
    [CreateAssetMenu(fileName = "Passive rune", menuName = "Rune/Passive")]
    public class PassiveRuneSO : RuneSO
    {
        [SerializeField] private List<BaseEffectFactorySO> effectsApplyWhenEquip;

        public IEnumerable<BaseEffectFactorySO> EffectsApplyWhenEquip => effectsApplyWhenEquip;

        public override IAbility GetAbility()
        {
            return new PassiveAbility(this);
        }

        public override string GetDescription()
        {
            var description = GenericPool<StringBuilder>.Get();
            description.Clear();
            description.AppendLine(this.Description);
            foreach (var effect in effectsApplyWhenEquip)
            {
                description.AppendLine(effect.EffectInfo.DesriptionWithColor);
            }

            string strDescription = description.ToString();
            GenericPool<StringBuilder>.Release(description);
            return strDescription;
        }

        private readonly static string[] _types = new string[] { "Rune", "Passive" };

        public override IEnumerable<string> GetSubTypes() => _types;
    }
}