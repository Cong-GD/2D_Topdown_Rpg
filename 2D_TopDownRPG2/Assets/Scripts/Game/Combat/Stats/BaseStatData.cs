using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Stats/Base")]
public class BaseStatData : ScriptableObject
{
    public float MaxHealth;
    public float MaxMana;
    public float MoveSpeed;
    public float AttackPower;
    public float Defence;
    public float BlockChance;
    public float Accuracy;
    public float Evasion;
    public float CriticalChance;
    public float CritticalHitDamage;

    public LvGrowStat growStat;
}

[Serializable]
public class LvGrowStat
{
    public float Health;
    public float Mana;
    public float AttackPower;
    public float Defence;

    public Dictionary<Stat, StatModifier> GetGrowingStat(int level)
    {
        var modifiers = new Dictionary<Stat, StatModifier>();

        if (Health > 0)
        {
            modifiers.Add(Stat.MaxHealth, new StatModifier(Health * level, StatModifier.BonusType.Flat, 0));
        }
        if (Mana > 0)
        {
            modifiers.Add(Stat.MaxMana, new StatModifier(Mana * level, StatModifier.BonusType.Flat, 0));
        }
        if (AttackPower > 0)
        {
            modifiers.Add(Stat.AttackPower, new StatModifier(AttackPower * level, StatModifier.BonusType.Flat, 0));
        }
        if (Defence > 0)
        {
            modifiers.Add(Stat.Defence, new StatModifier(Defence * level, StatModifier.BonusType.Flat, 0));
        }
        return modifiers;
    }
}