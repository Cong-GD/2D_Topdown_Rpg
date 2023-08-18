using CongTDev.IOSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CongTDev.AbilitySystem
{
    public abstract class RuneSO : ScriptableObject, IItem
    {
        public const string ITEM_TYPE = "Rune";

        [field: SerializeField] public string Name { get; private set; }

        [field: TextArea]
        [field: SerializeField] public string Description {get; private set; }

        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: SerializeField] public Sprite AbilityIcon { get; private set; }

        [field: SerializeField] public ItemRarity Rarity { get; private set; }

        public event Action OnDestroy;

        public string ItemType => ITEM_TYPE;

        public abstract IEnumerable<string> GetSubTypes();

        public abstract IAbility GetAbility();

        public abstract string GetDescription();

        public SerializedObject Serialize()
        {
            string path = FileNameData.GetRuneResourcePath(name);
            return new SerializedResourceAsset(path);
        }

        public void Destroy()
        {
            OnDestroy?.Invoke();
        }
    }
}