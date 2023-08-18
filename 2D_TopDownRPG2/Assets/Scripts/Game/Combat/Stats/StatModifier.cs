using System;

[Serializable]
public class StatModifier : IComparable<StatModifier>
{
    public enum BonusType
    {
        Flat = 100,
        PercentAdd = 200,
        PercentMulti = 300
    }

    public readonly float value;
    public readonly BonusType bonusType;
    public readonly int order;

    public StatModifier(float value, BonusType bonusType)
        : this(value, bonusType, (int)bonusType)
    {
    }

    public StatModifier(float value, BonusType bonusType, int order)
    {
        this.value = value;
        this.bonusType = bonusType;
        this.order = order;
    }

    public int CompareTo(StatModifier other)
    {
        return this.order.CompareTo(other.order);
    }
}