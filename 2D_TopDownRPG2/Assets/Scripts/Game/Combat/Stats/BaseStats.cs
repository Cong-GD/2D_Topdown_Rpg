using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats
{
    [SerializeField] private float _baseValue;

    public event Action<float> OnValueChange;
    private readonly List<StatModifier> _bonusStats = new();

    public float FinalValue { get; private set; }
    public float BaseValue
    {
        get => _baseValue;
        set
        {
            if (value <= 0) value = 0;
            _baseValue = value;
            CalculateFinalValue();
        }
    }

    public BaseStats()
    {
        _baseValue = 0;
    }

    public BaseStats(float baseValue)
    {
        this._baseValue = baseValue;
        CalculateFinalValue();
    }

    public void AddBonus(StatModifier bonus)
    {
        _bonusStats.Add(bonus);
        _bonusStats.Sort();
        CalculateFinalValue();
    }

    public void RemoveBonus(StatModifier stat)
    {
        _bonusStats.Remove(stat);
        CalculateFinalValue();
    }

    public void ClearAllBonus()
    {
        _bonusStats.Clear();
        CalculateFinalValue();
    }

    private void CalculateFinalValue()
    {
        float totalAddByPercent = 0;
        float sumBonus = _baseValue;
        foreach (StatModifier stat in _bonusStats)
        {
            if (stat.bonusType == StatModifier.BonusType.Flat)
            {
                sumBonus += stat.value;
            }
            else if (stat.bonusType == StatModifier.BonusType.PercentAdd)
            {
                totalAddByPercent += stat.value * sumBonus;
            }
            else
            {
                sumBonus += (sumBonus + totalAddByPercent) * (1 + stat.value);
            }
        }
        FinalValue = sumBonus + totalAddByPercent;
        if (FinalValue < 0) FinalValue = 0;
        OnValueChange?.Invoke(FinalValue);
    }
}

