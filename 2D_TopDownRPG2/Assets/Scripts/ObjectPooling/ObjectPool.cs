using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CongTDev.ObjectPooling
{
    public class ObjectPool
    {
        private readonly GameObject _prefab;

        private readonly Queue<IPoolObject> _pool = new();

        private readonly List<IPoolObject> _allInstance = new();

        public int CountActive => CountAll - CountInactive;
        public int CountInactive => _pool.Count;
        public int CountAll => _allInstance.Count;

        public ObjectPool(GameObject prefab)
        {
            if (!prefab.TryGetComponent<IPoolObject>(out _))
            {
                throw new ArgumentException("GameObject must have a component that implements IPoolObject", nameof(prefab));
            }
            _prefab = prefab;
        }

        public IPoolObject Get()
        {
            IPoolObject instance;
            if (_pool.Any())
            {
                instance = _pool.Dequeue();
                if ((MonoBehaviour)instance == null)
                {
                    instance = Instanciate();
                }
                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = Instanciate();
            }
            instance.Init(Release);
            return instance;
        }

        public void ClearPool()
        {
            foreach (var instance in _allInstance)
            {
                if ((MonoBehaviour)instance != null)
                {
                    UnityEngine.Object.Destroy(instance.gameObject);
                }
            }
            _pool.Clear();
            _allInstance.Clear();
        }

        private IPoolObject Instanciate()
        {
            var instance = UnityEngine.Object.Instantiate(_prefab).GetComponent<IPoolObject>();
            _allInstance.Add(instance);
            return instance;
        }

        private void Release(IPoolObject instance)
        {
            instance.gameObject.SetActive(false);
            _pool.Enqueue(instance);
        }
    }
}