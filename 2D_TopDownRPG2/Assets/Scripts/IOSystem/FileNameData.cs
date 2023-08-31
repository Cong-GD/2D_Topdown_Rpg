using CongTDev.EventManagers;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CongTDev.IOSystem
{
    public static class FileNameData
    {
        public readonly static string SavePath = Application.persistentDataPath;

        private static string _currentUser = "Test";
        public static string CurrentUser
        {
            get => _currentUser;
            set
            {
                if (!IsThisUserExits(value))
                {
                    AddUser(value);
                }
                _currentUser = value;
            }
        }

        public static void SetUser(string username)
        {
            CurrentUser = username;
        }

        public static string[] GetAllUser()
        {
            var names = Directory.EnumerateDirectories(SavePath).ToArray();
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = names[i][(names[i].LastIndexOf("\\") + 1)..];
            }
            return names;
        }

        public static bool DeleteUser(string username)
        {
            string path = Path.Combine(SavePath, username);
            if (Directory.Exists(path))
            {
                foreach(var file in Directory.GetFiles(path))
                {
                    File.Delete(file);
                }
                Directory.Delete(path);
                return true;
            }
            return false;
        }

        public static bool AddUser(string username)
        {
            string path = Path.Combine(SavePath, username);
            if (Directory.Exists(path))
                return false;

            Directory.CreateDirectory(path);
            return true;
        }

        public static bool IsThisUserHasWritten(string username)
        {
            string path = Path.Combine(SavePath, username);
            if (!Directory.Exists(path))
                return false;

            return Directory.GetFiles(path).Any();
        }

        public static bool IsThisUserExits(string username)
        {
            string path = Path.Combine(SavePath, username);
            return Directory.Exists(path);
        }


        public const string Equiments = "Equipment";
        public const string Abilities = "Ability";
        public const string Inventory = "Inventory";

        public static string GetFullPath(string filename)
        {
            return Path.Combine(SavePath, CurrentUser, filename);
        }

        public static string GetEffectResourcePath(string effectName)
        {
            return $"Effects/{effectName}";
        }

        public static string GetRuneResourcePath(string runeName)
        {
            return $"Runes/{runeName}";
        }

        public static string GetEquimentResourcePath(string equipmentName)
        {
            return $"Equipments/{equipmentName}";
        }

        public static string GetConsumableResourcePath(string consumableName)
        {
            return $"ConsumableItems/{consumableName}";
        }
    }

}