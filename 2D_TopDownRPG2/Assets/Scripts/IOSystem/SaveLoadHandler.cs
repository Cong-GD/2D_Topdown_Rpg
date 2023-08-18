using System.IO;

namespace CongTDev.IOSystem
{
    public static class SaveLoadHandler
    {
        public static void SaveToFile(string fileName, ISerializable serializableObject)
        {
            string path = FileNameData.GetFullPath(fileName);
            string json = serializableObject.ToWrappedJson();
            File.WriteAllText(path, json);
        }

        public static object LoadFromFile(string fileName)
        {
            if(!File.Exists(fileName))
            {
                return null;
            }
            string path = FileNameData.GetFullPath(fileName);
            string wrappedJson = File.ReadAllText(path);
            return JsonHelper.WrappedJsonToObject(wrappedJson);
        }
    }

}