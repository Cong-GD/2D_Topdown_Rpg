using System;
using System.Collections.Generic;

public enum Stat
{
    MaxHealth,
    MaxMana,
    MoveSpeed,
    AttackPower,
    Defence,
    BlockChance,
    Accuracy,
    Evasion,
    CriticalChance,
    CritticalHitDamage
}

[Serializable]
public class CharactorStat
{
    private readonly Dictionary<Stat, BaseStats> _stats;

    public BaseStats this[Stat stat] => _stats[stat];

    public CharactorStat()
    {
        _stats = new Dictionary<Stat, BaseStats>();
        foreach (Stat statType in Enum.GetValues(typeof(Stat)))
        {
            _stats[statType] = new BaseStats(0);
        }
    }

    public void SetStatBase(BaseStatData data)
    {
        _stats[Stat.MaxHealth].BaseValue = data.MaxHealth;
        _stats[Stat.MaxMana].BaseValue = data.MaxMana;
        _stats[Stat.MoveSpeed].BaseValue = data.MoveSpeed;
        _stats[Stat.AttackPower].BaseValue = data.AttackPower;
        _stats[Stat.Defence].BaseValue = data.Defence;
        _stats[Stat.BlockChance].BaseValue = data.BlockChance;
        _stats[Stat.Accuracy].BaseValue = data.Accuracy;
        _stats[Stat.Evasion].BaseValue = data.Evasion;
        _stats[Stat.CriticalChance].BaseValue = data.CriticalChance;
        _stats[Stat.CritticalHitDamage].BaseValue = data.CritticalHitDamage;
    }

    public void ClearAllBonus()
    {
        foreach (BaseStats baseStats in _stats.Values)
        {
            baseStats.ClearAllBonus();
        }
    }

    public void ApplyModifier(Stat statsType, StatModifier statBonus)
    {
        _stats[statsType].AddBonus(statBonus);
    }

    public void RemoveModifier(Stat statsType, StatModifier statBonus)
    {
        _stats[statsType].RemoveBonus(statBonus);
    }

    public BaseStats GetStats(Stat statType)
    {
        return _stats[statType];
    }
}
