using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace CongTDev.ObjectPooling
{
    public class PoolManager : GlobalReference<PoolManager>
    {
        private static readonly Dictionary<string, ObjectPool> _pools = new();

        public static bool Get<T>(Prefab prefab, out T instance) where T : IPoolObject
        {
            return Instance.InstanceGet(prefab, out instance);
        }

        public bool InstanceGet<T>(Prefab prefab, out T instance) where T : IPoolObject
        {
            try
            {
                if (!_pools.TryGetValue(prefab.UniquePrefabID, out var pool))
                {
                    pool = new ObjectPool(prefab.gameObject);
                    _pools[prefab.UniquePrefabID] = pool;
                }
                instance = (T)pool.Get();
                return true;
            }
            catch
            {
                instance = default;
                return false;
            }
        }
        protected override void Awake()
        {
            base.Awake();
            SceneManager.sceneUnloaded += ClearPool;
        }
        private void OnDestroy()
        {
            SceneManager.sceneUnloaded -= ClearPool;
        }

        private static void ClearPool(Scene arg0)
        {
            foreach (var pool in _pools.Values)
            {
                pool.ClearPool();
            }
        }
    }
}