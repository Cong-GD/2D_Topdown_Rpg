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
                OnInputChange?.Invoke(value);
                _inputVector = value;
            }
        }
    }

    public event Action<Vector2> OnInputChange;
}
