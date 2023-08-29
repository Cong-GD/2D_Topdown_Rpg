using System;
using System.IO;
using UnityEngine;

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
            string path = FileNameData.GetFullPath(fileName);
            if (!File.Exists(path))
            {
                return null;
            }

            string wrappedJson = File.ReadAllText(path);
            return JsonHelper.WrappedJsonToObject(wrappedJson);
        }

        public static void SaveToFile(string fileName, object data)
        {
            string path = FileNameData.GetFullPath(fileName);
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(path, json);
        }
        public static T LoadFromFile<T>(string fileName) where T : class
        {
            string path = FileNameData.GetFullPath(fileName);
            if (!File.Exists(path))
            {
                return null;
            }
            try
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<T>(json);
            }
            catch
            {
                return null;
            }
            
        }
    }

}