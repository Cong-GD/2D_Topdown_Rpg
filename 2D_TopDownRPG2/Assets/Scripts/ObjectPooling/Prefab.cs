using CongTDev.AbilitySystem.Spell;
using UnityEngine;

namespace CongTDev.ObjectPooling
{
    [RequireComponent(typeof(IPoolObject))]
    public class Prefab : MonoBehaviour
    {
        [field: SerializeField] public string UniquePrefabID { get; private set; }
    }
}