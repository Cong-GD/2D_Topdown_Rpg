using CongTDev.AbilitySystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Item/BasicEquipment")]
public class BasicEquipmentFactory : EquipmentFactory
{

    [SerializeField] private List<BaseEffectFactory> subEffects;

    public override IItem CreateItem()
    {
        var equipment = new Equipment(this, MainStats.ToArray());
        foreach (var effect in subEffects)
        {
            equipment.AddSubEffect(effect);
        }
        return equipment;
    }
}