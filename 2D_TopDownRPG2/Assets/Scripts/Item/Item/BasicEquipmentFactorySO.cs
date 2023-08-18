using CongTDev.AbilitySystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Item/BasicEquipment")]
public class BasicEquipmentFactorySO : EquipmentFactorySO
{

    [SerializeField] private List<BaseEffectFactorySO> subEffects;

    public override Equipment CreateEquipment()
    {
        var equipment = new Equipment(this, MainStats.ToArray());
        foreach (var effect in subEffects)
        {
            equipment.AddSubEffect(effect);
        }
        return equipment;
    }
}