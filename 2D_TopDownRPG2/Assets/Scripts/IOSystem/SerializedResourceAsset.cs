using System;
using UnityEngine;

namespace CongTDev.IOSystem
{
    [Serializable]
    public class SerializedResourceAsset : SerializedObject
    {
        public string objectPath;

        public SerializedResourceAsset(string objectPath)
        {
            this.objectPath = objectPath;
        }

        public override object Deserialize()
        {
            return Resources.Load(objectPath);
        }

        public override SerializedType GetSerializedType() => SerializedType.ResourceAsset;
    }
}