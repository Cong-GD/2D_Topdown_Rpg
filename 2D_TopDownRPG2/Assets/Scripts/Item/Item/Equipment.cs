using CongTDev.AbilitySystem;
using CongTDev.IOSystem;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Equipment : IItem
{
    public const string ITEM_TYPE = "Equipment";
    private static readonly int rarityCount = Enum.GetNames(typeof(ItemRarity)).Length;

    public event Action OnDestroy;

    private readonly BuffEffectFactory[] mainStats;
    private readonly List<BaseEffectFactory> subEffects;
    private readonly EquipmentFactory sourceInfo;
    private readonly List<IEffect> appliedEffect = new();
    private readonly List<string> _types = new();
    private Fighter equiper;

    public Sprite Icon => sourceInfo.Icon;
    public ItemRarity Rarity => (ItemRarity)Mathf.Min(subEffects.Count, rarityCount - 1);
    public string Name => sourceInfo.EquipmentName;
    public Slot EquipSlot => sourceInfo.EquipSlot;
    public string ItemType => ITEM_TYPE;

    public Equipment(EquipmentFactory sourceInfo, BuffEffectFactory[] mainStats)
    {
        this.mainStats = mainStats;
        this.sourceInfo = sourceInfo;
        _types.Add(sourceInfo.EquipSlot.ToString());
        subEffects = new();
    }

    public IEnumerable<string> GetSubTypes() => _types;

    public void Destroy()
    {
        OnDestroy?.Invoke();
    }

    public void AddSubEffect(BaseEffectFactory subEffect)
    {
        if(subEffect == null)
            return;

        subEffects.Add(subEffect);
        var effectType = subEffect.EffectInfo.EffectTypeInfo;
        if (!string.IsNullOrEmpty(effectType) && !_types.Contains(effectType))
        {
            _types.Add(effectType);
        }
    }

    public string GetDescription()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(sourceInfo.Description);
        stringBuilder.AppendLine("Main stats:");
        foreach (var effect in mainStats)
        {
            stringBuilder.AppendLine(effect.EffectInfo.DesriptionWithColor);
        }
        if (subEffects.Count == 0)
            return stringBuilder.ToString();

        stringBuilder.AppendLine("Bonus effect: ");
        foreach (var effect in subEffects)
        {
            stringBuilder.AppendLine(effect.EffectInfo.DesriptionWithColor);
        }
        return stringBuilder.ToString();
    }

    public void Equip(Fighter equiper)
    {
        this.equiper = equiper;
        foreach (var effectFactory in mainStats)
        {
            var effect = effectFactory.Build();
            equiper.ReceiveEffect(effect, equiper);
            appliedEffect.Add(effect);
        }
        foreach (var effectFactory in subEffects)
        {
            var effect = effectFactory.Build();
            equiper.ReceiveEffect(effect, equiper);
            appliedEffect.Add(effect);
        }
    }

    public void Unequip()
    {
        foreach (var effect in appliedEffect)
        {
            equiper.RemoveEffect(effect);
        }
        appliedEffect.Clear();
    }

    public enum Slot
    {
        Weapon,
        Armor,
        Shoe,
        Shield
    }
    #region IOSystem
    public SerializedObject Serialize()
    {
        return new SerializedEquipment(this);
    }

    [Serializable]
    public class SerializedEquipment : SerializedObject
    {
        public string[] mainEffectJsons;
        public string[] subEffectsJsons;
        public string sourceInfoJson;

        public SerializedEquipment() { }

        public SerializedEquipment(Equipment equipment)
        {
            sourceInfoJson = equipment.sourceInfo.ToWrappedJson();
            mainEffectJsons = new string[equipment.mainStats.Length];
            for (int i = 0; i < equipment.mainStats.Length; i++)
            {
                mainEffectJsons[i] = equipment.mainStats[i].ToWrappedJson();
            }
            subEffectsJsons = new string[equipment.subEffects.Count];
            for (int i = 0; i < equipment.subEffects.Count; i++)
            {
                subEffectsJsons[i] = equipment.subEffects[i].ToWrappedJson();
            }
        }

        public override object Deserialize()
        {
            var mainEffects = new BuffEffectFactory[mainEffectJsons.Length];
            for (int i = 0; i < mainEffects.Length; i++)
            {
                mainEffects[i] = (BuffEffectFactory)JsonHelper.WrappedJsonToObject(mainEffectJsons[i]);
            }

            var sourceInfo = (EquipmentFactory)JsonHelper.WrappedJsonToObject(sourceInfoJson);
            var equipment = new Equipment(sourceInfo, mainEffects);

            foreach (var subEffectsJson in subEffectsJsons)
            {
                var effectFactory = (BaseEffectFactory)JsonHelper.WrappedJsonToObject(subEffectsJson);
                equipment.AddSubEffect(effectFactory);
            }

            return equipment;
        }

        public override SerializedType GetSerializedType() => SerializedType.Equipment;
    }
    #endregion

}