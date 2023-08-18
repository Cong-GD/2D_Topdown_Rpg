using CongTDev.AbilitySystem;
using CongTDev.IOSystem;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentFactorySO : ScriptableObject, ISerializable
{
    [field: SerializeField] public string EquipmentName { get; private set; }

    [field: SerializeField] public Equipment.Slot EquipSlot { get; private set; }

    [field: TextArea]
    [field: SerializeField] public string Description { get; private set; }

    [field: SerializeField] public Sprite Icon { get; private set; }

    [field: SerializeField] public List<BuffEffectFactory> MainStats { get; private set; }

    public abstract Equipment CreateEquipment();

    public SerializedObject Serialize()
    {
        string path = FileNameData.GetEquimentResourcePath(name);
        return new SerializedResourceAsset(path);
    }
}