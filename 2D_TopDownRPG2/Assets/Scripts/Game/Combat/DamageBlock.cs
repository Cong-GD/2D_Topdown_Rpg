using System;
using System.Collections.Generic;

public enum DamageState
{
    NormalDamage,
    CriticalDamage,
    BlockDamage,
    Miss
}

public enum DamageType
{
    PhysicalDamage,
    MagicDamage,
    EnviromentDamage
}

public class DamageBlock
{
    #region Static Member
    private const float BLOCKMULTILIER = -0.7f;
    private static readonly IReadOnlyDictionary<DamageType, IEnumerable<Action<DamageBlock>>> _actionMap
        = new Dictionary<DamageType, IEnumerable<Action<DamageBlock>>>()
    {
        {
            DamageType.PhysicalDamage, new Action<DamageBlock>[]
            {
                CriticalCalculater,
                BlockCalculator,
                MissCalculator

            }
        },
        {
            DamageType.MagicDamage, new Action<DamageBlock>[]
            {
                CriticalCalculater,
                BlockCalculator,
                MissCalculator

            }
        },
            {
            DamageType.EnviromentDamage, new Action<DamageBlock>[]
            {
            }
        }
    };

    private static void MissCalculator(DamageBlock damageBlock)
    {
        CharactorStat targetStat = damageBlock.Target.Stats;

        float missRate = targetStat[Stat.Evasion].FinalValue - damageBlock.Source.Stats[Stat.Accuracy].FinalValue;

        if (!Chance.TryOnPercent(missRate))
            return;

        damageBlock.AddMutiplier(-10f);
        damageBlock.State = DamageState.Miss;
    }

    private static void BlockCalculator(DamageBlock damageBlock)
    {
        CharactorStat targetStat = damageBlock.Target.Stats;

        float blockRate = targetStat[Stat.BlockChance].FinalValue - damageBlock.Source.Stats[Stat.Accuracy].FinalValue;

        if (!Chance.TryOnPercent(blockRate))
            return;

        damageBlock.AddMutiplier(BLOCKMULTILIER);
        damageBlock.State = DamageState.BlockDamage;
    }

    private static void CriticalCalculater(DamageBlock damageBlock)
    {
        if (!Chance.TryOnPercent(damageBlock.Source.Stats[Stat.CriticalChance].FinalValue))
            return;

        damageBlock.AddMutiplier(damageBlock.Source.Stats[Stat.CritticalHitDamage].FinalValue / 100);
        damageBlock.State = DamageState.CriticalDamage;
    }
    #endregion

    public DamageType DamageType { get; private set; }
    public float RawDamage { get; private set; }
    public DamageState State { get; private set; }
    public Fighter Source { get; private set; }
    public Fighter Target { get; private set; }
    public float CurrentDamage { get; private set; }
    public float Multiplier { get; private set; }

    public void Init(DamageType damageType, Fighter source, float damage)
    {
        State = DamageState.NormalDamage;
        DamageType = damageType;
        RawDamage = damage;
        Source = source;
        Multiplier = 1f;
        CurrentDamage = damage;
    }

    public void Init(float damage)
    {
        DamageType = DamageType.EnviromentDamage;
        RawDamage = damage;
        Multiplier = 1f;
        CurrentDamage = damage;
    }

    public void AddMutiplier(float value)
    {
        Multiplier += value;
        CurrentDamage = RawDamage * Multiplier;
        State = DamageState.NormalDamage;
        if (CurrentDamage < 0) CurrentDamage = 0;
    }

    public void CalculateFinalDamage(Fighter target)
    {
        if (RawDamage <= 0 || target == null)
            return;

        Target = target;
        var damageCalculators = _actionMap[DamageType];
        foreach (var damageCalculator in damageCalculators)
        {
            damageCalculator.Invoke(this);
        }
    }
}