using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Extension
{

    public static Action WrapAction<T>(this Action<T> action, T parameter)
    {
        return () => action?.Invoke(parameter);
    }

    public static Action UnityEventToAction(this UnityEvent action)
    {
        return () => action.Invoke();
    }
}

public static class VectorHelper
{
    public static List<Vector2> SpreadDirectionCenter(Vector2 direction, int count, float spreadAngle)
    {
        float fireAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireAngle += spreadAngle / 2;
        float differentAngle = spreadAngle / count;
        var result = new List<Vector2>();
        for (int i = 0; i < count; i++)
        {
            fireAngle -= differentAngle;
            float fireRad = Mathf.Repeat(fireAngle, 360) * Mathf.Deg2Rad;
            Vector2 fireDirect = new(Mathf.Cos(fireRad), Mathf.Sin(fireRad));
            result.Add(fireDirect);
        }
        return result;
    }
    public static List<Vector2> SpreadDirectionAdd(Vector2 direction, int count, float spreadAngle)
    {
        float fireAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float differentAngle = spreadAngle / count;
        var result = new List<Vector2>();
        for (int i = 0; i < count; i++)
        {
            fireAngle = (fireAngle + differentAngle) % 360;
            float fireRad = fireAngle * Mathf.Deg2Rad;
            Vector2 fireDirect = new(Mathf.Cos(fireRad), Mathf.Sin(fireRad));
            result.Add(fireDirect);
        }
        return result;
    }
}

public static class Chance
{
    public static bool TryOnPercent(float chanceOnPercent)
    {
        return UnityEngine.Random.Range(0, 100f) <= chanceOnPercent;
    }

    public static bool TryOnValue(float chanceOnValue)
    {
        return UnityEngine.Random.value <= chanceOnValue;
    }
}
