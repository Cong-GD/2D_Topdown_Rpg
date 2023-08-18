using System;
using UnityEngine;

namespace CongTDev.ObjectPooling
{
    public interface IPoolObject
    {
        GameObject gameObject { get; }
        void Init(Action<IPoolObject> returnAction);

        void ReturnToPool();
    }
}