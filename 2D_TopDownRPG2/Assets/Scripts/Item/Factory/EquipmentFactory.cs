using CongTDev.AbilitySystem;
using CongTDev.IOSystem;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentFactory : BaseItemFactory, ISerializable
{
    [field: SerializeField] public string EquipmentName { get; private set; }

    [field: SerializeField] public Equipment.Slot EquipSlot { get; private set; }

    [field: TextArea]
    [field: SerializeField] public string Description { get; private set; }

    [field: SerializeField] public Sprite Icon { get; private set; }

    [field: SerializeField] public List<BuffEffectFactory> MainStats { get; private set; }

    public SerializedObject Serialize()
    {
        string path = FileNameData.GetEquimentResourcePath(name);
        return new SerializedResourceAsset(path);
    }
}
