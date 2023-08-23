using CongTDev.IOSystem;
using UnityEngine;

namespace CongTDev.AbilitySystem
{
    public abstract class BaseEffectFactory : ScriptableObject, ISerializable
    {
        [field: SerializeField] public EffectInfo EffectInfo { get; private set; } 

        public abstract IEffect Build();

        public SerializedObject Serialize()
        {
            string path = FileNameData.GetEffectResourcePath(name);
            return new SerializedResourceAsset(path);
        }
    }
}