using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace CongTDev.ObjectPooling
{
    public class PrefixPool : MonoBehaviour
    {
        [Serializable]
        public class PoolInfo
        {
            public Prefab prefab;
            public int amount;
        }

        [SerializeField] private List<PoolInfo> poolInfos;

        private void Start()
        {
            foreach (var poolInfo in poolInfos)
            {
                if (poolInfo.prefab == null)
                    continue;
                var intances = ListPool<IPoolObject>.Get();
                for (int i = 0; i < poolInfo.amount; i++)
                {
                    if (PoolManager.Get<IPoolObject>(poolInfo.prefab, out var intance))
                    {
                        intance.gameObject.transform.SetParent(transform);
                        intances.Add(intance);
                    }
                }
                foreach (var intance in intances)
                {
                    intance.ReturnToPool();
                }
                ListPool<IPoolObject>.Release(intances);
            }
        }
    }
}