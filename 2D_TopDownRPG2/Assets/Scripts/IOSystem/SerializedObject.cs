using System;

namespace CongTDev.IOSystem
{
    [Serializable]
    public abstract class SerializedObject
    {
        public abstract SerializedType GetSerializedType();
        public abstract object Deserialize();
    }

    [Serializable]
    public class SerializedNullObject : SerializedObject
    {
        public override object Deserialize()
        {
            return null;
        }

        public override SerializedType GetSerializedType() => SerializedType.Null;

        public readonly static SerializedNullObject intance = new();
    }
}