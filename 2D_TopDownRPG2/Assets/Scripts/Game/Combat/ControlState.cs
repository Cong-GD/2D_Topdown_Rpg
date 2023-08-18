using System;

public class ControlState
{
    public event Action OnStun;

    private int _ccPoint;
    private int _currentPoint;

    public bool IsFree()
    {
        return true;
    }

    public void ReceiveCC(int point)
    {
        _currentPoint += point;
        if(_currentPoint >= _ccPoint)
        {
            OnStun?.Invoke();
        }
    }
}