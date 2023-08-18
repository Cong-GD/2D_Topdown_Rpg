using UnityEngine;

public class GlobalReference<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _reference;
    public static T Instance 
    {
        get
        {
            if( _reference == null)
            {
                _reference = FindAnyObjectByType<T>();
            }
            return _reference;
        }
    }

    protected virtual void Awake()
    {
        if(_reference != null && !ReferenceEquals(_reference, this))
            Destroy(gameObject);
        else
        {
            _reference = (T)(MonoBehaviour)this;
        }    
    }
}
