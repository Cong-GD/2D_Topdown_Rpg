using CongTDev.AbilitySystem;
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
    public float CooldownReduction;
}

