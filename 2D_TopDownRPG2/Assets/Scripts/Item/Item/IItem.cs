using CongTDev.IOSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IItem : ISerializable
{
    event Action OnDestroy;
    Sprite Icon { get; }
    ItemRarity Rarity { get; }
    string Name { get; }
    string ItemType { get; }
    IEnumerable<string> GetSubTypes();
    void Destroy();
    string GetDescription();
}