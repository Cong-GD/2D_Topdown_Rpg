using System.Collections.Generic;
using UnityEngine;

public static class CoroutineHelper
{
    private const int MAX_INTANCE_COUNT = 300;

    public static readonly WaitForFixedUpdate fixedUpdateWait = new();
    public static readonly WaitForEndOfFrame endOfFrameWait = new();

    private static readonly Dictionary<float, WaitForSeconds> _secondsWaitMap = new();
    private static readonly Dictionary<float, WaitForSecondsRealtime> _realtimeSecondsWaitMap = new();

    /// <summary>
    /// Return WaitForSeconds object use for Coroutine
    /// </summary>
    public static WaitForSeconds Wait(this float waitTime)
    {
        if (!_secondsWaitMap.TryGetValue(waitTime, out var wait))
        {
            if(waitTime < Mathf.Epsilon)
            {
                return null;
            }
            wait = new(waitTime);
            _secondsWaitMap[waitTime] = wait;
            if(_secondsWaitMap.Count > MAX_INTANCE_COUNT)
            {
                _secondsWaitMap.Clear();
            }
        }
        return wait;
    }

    public static WaitForSecondsRealtime WaitInRealTime(this float waitTime)
    {
        if (!_realtimeSecondsWaitMap.TryGetValue(waitTime, out var wait))
        {
            if (waitTime < Mathf.Epsilon)
            {
                return null;
            }
            wait = new(waitTime);
            _realtimeSecondsWaitMap[waitTime] = wait;
            if (_realtimeSecondsWaitMap.Count > MAX_INTANCE_COUNT)
            {
                _realtimeSecondsWaitMap.Clear();
            }
        }
        return wait;
    }

}
