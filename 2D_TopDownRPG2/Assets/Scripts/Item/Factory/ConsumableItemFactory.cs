using CongTDev.AbilitySystem;
using CongTDev.IOSystem;
using CongTDev.ObjectPooling;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Item/Consumable")]
public class ConsumableItemFactory : BaseItemFactory, ISerializable
{
    [field: SerializeField] public Sprite Icon { get; private set; }

    [field: SerializeField] public string ItemName { get; private set; }

    [field: TextArea]
    [field: SerializeField] public string Description { get; private set; }

    [field: SerializeField] public ItemRarity ItemRarity { get; private set; }

    [field: SerializeField] public int MaxStack { get; private set; }


    [field: SerializeField] private List<BaseEffectAndFactory> effects;


    [SerializeField] private List<string> types;

    [field: SerializeField] public Prefab VFXWhenUse { get; private set; }
    [field: SerializeField] public string SFXWhenUse { get; private set; }

    public IEnumerable<string> Types => types;

    public IEnumerable<BaseEffectAndFactory> Effects => effects;

    public override IItem CreateItem()
    {
        return new ConsumableItem(this);
    }

    public SerializedObject Serialize()
    {
        string path = FileNameData.GetConsumableResourcePath(name);
        return new SerializedResourceAsset(path);
    }
}