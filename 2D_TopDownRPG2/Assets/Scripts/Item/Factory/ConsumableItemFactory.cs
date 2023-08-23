using CongTDev.AbilitySystem;
using CongTDev.IOSystem;
using CongTDev.RequimentSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Item/Consumable")]
public class ConsumableItemFactory : BaseItemFactory , ISerializable
{
    [field: SerializeField] public Sprite Icon { get; private set; }

    [field: SerializeField] public string ItemName { get; private set; }

    [field: SerializeField] public string Description { get; private set; }

    [field: SerializeField] public ItemRarity ItemRarity { get; private set; }

    [field: SerializeField] public int MaxStack { get; private set; }


    [field: SerializeField] private List<BaseEffectAndFactorySO> effects;


    [SerializeField] private List<string> types;


    [SerializeField] private Requiment[] useRequiment;

    public IEnumerable<string> Types => types;

    public IEnumerable<BaseEffectAndFactorySO> Effects => effects;

    public IEnumerable<IRequiment> UseRequiment => useRequiment;

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