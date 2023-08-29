using CongTDev.AbilitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Fighter : MonoBehaviour
{
    [field: SerializeField] public Collider2D HitBox { get; private set; }
    [field: SerializeField] public Movement Movement { get; private set; }

    public readonly ResourceBlock Health = new();
    public readonly ResourceBlock Mana = new();
    public readonly CharactorStat Stats = new();

    public enum Team
    {
        Hero,
        Monster
    }

    public Team team;
    public event Action<DamageBlock> OnDealDamage;
    public event Action<DamageBlock> OnTakeDamage;
    public event Action<Fighter> OnDead;

    public bool IsInvisibility => Health.IsEmpty || invisibility;

    public bool invisibility;

    public Vector2 Position => HitBox.bounds.center;

    private readonly HashSet<IEffect> _effects = new();

    private void Reset()
    {
        if (HitBox == null)
        {
            HitBox = GetComponent<Collider2D>();
        }
    }

    protected virtual void Awake()
    {
        Stats[Stat.MaxHealth].OnValueChange += (value) => Health.Capacity = value;
        Stats[Stat.MoveSpeed].OnValueChange += (value) => Movement.MoveSpeed = value;
        Stats[Stat.MaxMana].OnValueChange += (value) => Mana.Capacity = value;
        Health.OnValueChange += CheckForDeath;
    }

    public void ReceiveEffect(IEffect effect, Fighter contributer)
    {
        effect.Instanciate(contributer, this);
        _effects.Add(effect);
    }

    public void RemoveEffect(IEffect effect)
    {
        if (_effects.Remove(effect))
        {
            effect.CleanUp();
        }
    }

    public void RemoveEffect(Func<IEffect, bool> predicate)
    {
        IEnumerable<IEffect> deletedEffects = _effects.Where(effect => predicate(effect));
        foreach (IEffect effect in deletedEffects)
        {
            RemoveEffect(effect);
        }
    }

    public void RemoveAllEffect()
    {
        foreach (IEffect effect in _effects)
        {
            effect.CleanUp();
        }
        _effects.Clear();
    }

    public void TakeDamage(DamageBlock damageBlock)
    {
        if (IsInvisibility)
            return;

        damageBlock.CalculateFinalDamage(this);
        if(damageBlock.DamageType != DamageType.EnviromentDamage)
        {
            damageBlock.Source.OnDealDamage?.Invoke(damageBlock);
        }
        damageBlock.Target.OnTakeDamage?.Invoke(damageBlock);
        Health.Draw(damageBlock.CurrentDamage);
        DamageFeedback.Display(damageBlock);
    }

    public void InstanciateFromStatsData(BaseStatData baseData)
    {
        Stats.SetStatBase(baseData);
    }

    private void CheckForDeath(float current, float max)
    {
        if (current <= Mathf.Epsilon)
        {
            OnDead?.Invoke(this);
        }
    }  
}
