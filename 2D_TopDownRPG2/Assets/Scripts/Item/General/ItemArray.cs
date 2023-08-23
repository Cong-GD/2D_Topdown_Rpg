using CongTDev.IOSystem;

public class ItemArray : SerializedObject, ISerializable
{
    private static readonly ItemArray instance = new();

    public static ItemArray GetIntance(IItem[] items)
    {
        instance.items = items;
        return instance;
    }

    public IItem[] items;

    public JsonWrapper[] itemWrapper;

    public override object Deserialize()
    {
        if(itemWrapper == null)
            return null;

        items = new IItem[itemWrapper.Length];
        for(int i = 0; i < itemWrapper.Length; i++)
        {
            items[i] = (IItem)itemWrapper[i].ToObject();
        }
        return this;
    }

    public override SerializedType GetSerializedType() => SerializedType.ItemArray;

    public SerializedObject Serialize()
    {
        if(items == null) 
            return SerializedNullObject.intance;

        itemWrapper = new JsonWrapper[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            itemWrapper[i] = items[i].ToJsonWrapper();
        }
        return this;
    }
}