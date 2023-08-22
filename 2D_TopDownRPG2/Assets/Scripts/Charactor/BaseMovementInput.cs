using System;
using UnityEngine;

public abstract class BaseMovementInput : MonoBehaviour
{
    private Vector2 _inputVector;
    public Vector2 InputVector 
    {
        get => _inputVector;
        protected set
        {
            if (!value.Equals(_inputVector))
            {
                _inputVector = value;
                OnInputChange?.Invoke(value);
            }
        }
    }

    public event Action<Vector2> OnInputChange;
}
