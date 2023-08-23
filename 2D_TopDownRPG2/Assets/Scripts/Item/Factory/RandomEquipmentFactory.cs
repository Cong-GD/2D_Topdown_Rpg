using CongTDev.AbilitySystem;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Item/RandomStatEquipment")]
public class RandomEquipmentFactory : EquipmentFactory
{
    [SerializeField] private List<RandomChanceEffect> randomEffects;

    [Serializable]
    public struct RandomChanceEffect
    {
        public BaseEffectFactory effectFactory;
        [Range(0, 100)]
        public float chanceToAppear;
    }

    public override IItem CreateItem()
    {
        var equipment = new Equipment(this, MainStats.ToArray());
        foreach (var ramdonEffect in randomEffects)
        {
            if(Chance.TryOnPercent(ramdonEffect.chanceToAppear))
            {
                equipment.AddSubEffect(ramdonEffect.effectFactory);
            }
        }
        return equipment;
    }
}
