using System;
using UnityEngine;


[System.Serializable]
public class ResourceBlock
{
    private float _current;
    private float _capacity;

    public event Action<float, float> OnValueChange;

    public float Ratio
        => _capacity == 0 ? 0 : _current / _capacity;
    public bool IsEmpty => _current <= Mathf.Epsilon;
    public bool IsFull => _capacity - _current <= Mathf.Epsilon;

    public float Current
    {
        get { return _current; }
        set
        {
            _current = Mathf.Clamp(value, 0, _capacity);
            OnValueChange?.Invoke(Current, Capacity);
        }
    }

    public float Capacity
    {
        get => _capacity;
        set
        {
            if (value < 0)
                value = 0;
            _capacity = value;
            _current = Mathf.Clamp(_current, 0, _capacity);
            OnValueChange?.Invoke(Current, Capacity);
        }
    }

    public ResourceBlock()
    {
        _current = 1;
        _capacity = 1;
    }

    // This function use for set the value without trigged event. Please use it carefully
    public void ResetValue(float current, float capacity)
    {
        if(capacity < 0) capacity = 0;
        _capacity = capacity;
        _current = Mathf.Min(current, _capacity);
    }

    public void Fill()
    {
        Current = Capacity;
    }

    public void Recover(float value)
    {
        if (value <= 0) return;
        Current += value;
    }

    public void RecorverByRatio(float ratio)
    {
        float value = Capacity * ratio;
        Recover(value);
    }
    
    public void Draw(float value) 
    {
        if (value <= 0) return;
        Current -= value;
    }

    public void DrawByRatio(float ratio)
    {
        float value = Capacity * ratio;
        Draw(value);
    }
    
}
