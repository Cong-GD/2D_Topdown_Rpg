using System;
using UnityEngine;

namespace CongTDev.AbilitySystem
{
    [Serializable]
    public class StatBasedValue
    {
        [field: SerializeField] public Stat BaseOn {get; private set;}

        [field: SerializeField] public float PercentValue { get; private set;}

        [field: SerializeField] public float FixedValue { get; private set; }

        public float GetRawValue(Fighter source)
        {
            return source.Stats[BaseOn].FinalValue * PercentValue + FixedValue;
        }
    }
}