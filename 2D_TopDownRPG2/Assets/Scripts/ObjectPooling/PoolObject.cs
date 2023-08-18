using System;
using UnityEngine;

namespace CongTDev.ObjectPooling
{
    public class PoolObject : MonoBehaviour, IPoolObject
    {
        private Action<IPoolObject> _returnAction;

        public void Init(Action<IPoolObject> returnAction)
        {
            _returnAction = returnAction;
        }

        public void ReturnToPool()
        {
            if(_returnAction != null)
            {
                _returnAction.Invoke(this);
                _returnAction = null;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}