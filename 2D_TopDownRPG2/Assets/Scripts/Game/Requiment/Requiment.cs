using UnityEngine;

namespace CongTDev.RequimentSystem
{
    public abstract class Requiment : ScriptableObject, IRequiment
    {
        [SerializeField]
        private string description;

        public string Description => description;
        public abstract bool CheckForRequiment();
        public abstract bool CheckForRequiment(object user);
    }

    public interface IRequiment
    {
        bool CheckForRequiment();
        bool CheckForRequiment(object user);
    }
}