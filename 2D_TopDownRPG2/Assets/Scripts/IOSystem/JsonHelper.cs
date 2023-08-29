using CongTDev.AbilitySystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace CongTDev.IOSystem
{
    public static class JsonHelper
    {
        private static readonly Dictionary<SerializedType, Type> typeMap = new()
        {
            { SerializedType.Equipment, typeof(Equipment.SerializedEquipment) },
            { SerializedType.ResourceAsset, typeof(SerializedResourceAsset) },
            { SerializedType.Null, typeof(SerializedNullObject) },
            { SerializedType.PassiveAbility, typeof(PassiveAbility.PassiveAbilitySerialize) },
            { SerializedType.OrientationAbility, typeof(OrientationAbility.SerializedOrientationAbility) },
            { SerializedType.EquipmentManager, typeof(EquipmentManager.SerializedEquipmentManager) },
            { SerializedType.SkillSet, typeof(SkillSet.SerializedSkillSet) },
            { SerializedType.SeflActiveAbility, typeof(SelfActiveAbility.SerializedSeflActiveAbility) },
            { SerializedType.TargetingAbility, typeof(TargetingAbility.SerializedTargetingAbility) },
            { SerializedType.ConsumableItem, typeof(ConsumableItem.SerializedConsumableItem) },
            { SerializedType.ItemArray, typeof(ItemArray) },

        };

        public static Type ToSystemType(this SerializedType serializedType) => typeMap[serializedType];

        public static string ToWrappedJson(this ISerializable serializableObject)
        {
            var wrapper = GenericPool<JsonWrapper>.Get();
            OverideToJsonWrapper(serializableObject, ref wrapper);
            string wrappedJson = ToJson(wrapper);
            GenericPool<JsonWrapper>.Release(wrapper);
            return wrappedJson;
        }

        public static object WrappedJsonToObject(string wrappedJson)
        {
            try
            {
                var wrapper = JsonUtility.FromJson<JsonWrapper>(wrappedJson);
                return ToObject(wrapper);
            }
            catch
            {
                Debug.LogError("Wrong json format. This function use for wrapped json only");
            }
            return null;
        }

        public static void OverideToJsonWrapper(this ISerializable serializableObject, ref JsonWrapper wrapper)
        {
            if (serializableObject == null)
            {
                wrapper.type = SerializedType.Null;
                wrapper.json = string.Empty;
                return;
            }

            var serializedObject = serializableObject.Serialize();
            wrapper.type = serializedObject.GetSerializedType();
            wrapper.json = JsonUtility.ToJson(serializedObject);
        }

        public static JsonWrapper ToJsonWrapper(this ISerializable serializableObject)
        {
            var wrapper = new JsonWrapper();
            OverideToJsonWrapper(serializableObject, ref wrapper);
            return wrapper;
        }

        public static object ToObject(this JsonWrapper wrapper)
        {
            if (wrapper is null || wrapper.type == SerializedType.Null)
                return null;

            var serializedObject = (SerializedObject)JsonUtility.FromJson(wrapper.json, wrapper.type.ToSystemType());
            return serializedObject?.Deserialize();
        }

        public static string ToJson(this JsonWrapper wrapper)
        {
            return JsonUtility.ToJson(wrapper);
        }
    }

    [Serializable]
    public enum SerializedType
    {
        Null,
        Equipment,
        ResourceAsset,
        PassiveAbility,
        OrientationAbility,
        EquipmentManager,
        SkillSet,
        SeflActiveAbility,
        TargetingAbility,
        ConsumableItem,
        ItemArray
    }

    [Serializable]
    public class JsonWrapper
    {
        public SerializedType type;
        public string json;
    }

}