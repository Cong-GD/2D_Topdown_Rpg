using CongTDev.IOSystem;
using System;

[Serializable]
public class ItemArray : SerializedObject, ISerializable
{
    private static readonly ItemArray instance = new();

    public static ItemArray GetIntance(IItem[] items)
    {
        instance.items = items;
        return instance;
    }

    [NonSerialized]
    public IItem[] items;

    public string[] wrappedJson;

    public override object Deserialize()
    {
        if(wrappedJson == null)
            return null;

        items = new IItem[wrappedJson.Length];
        for(int i = 0; i < wrappedJson.Length; i++)
        {
            items[i] = (IItem)JsonHelper.WrappedJsonToObject(wrappedJson[i]);
        }
        return this;
    }

    public override SerializedType GetSerializedType() => SerializedType.ItemArray;

    public SerializedObject Serialize()
    {
        if(items == null) 
            return SerializedNullObject.intance;

        wrappedJson = new string[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            wrappedJson[i] = items[i].ToWrappedJson();
        }
        return this;
    }
}