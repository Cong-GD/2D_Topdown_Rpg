using System;
using System.Collections.Generic;

namespace CongTDev.EventManagers
{
    public static class EventManager
    {
        private static readonly Dictionary<string, Action> events = new();

        public static void RaiseEvent(string eventName)
        {
            if (events.TryGetValue(eventName, out Action action))
            {
                action?.Invoke();
            }
        }

        public static void AddListener(string eventName, Action action)
        {
            if (events.ContainsKey(eventName))
            {
                events[eventName] += action;
            }
            else
            {
                events.Add(eventName, action);
            }
        }

        public static void RemoveListener(string eventName, Action action)
        {
            if (events.ContainsKey(eventName))
            {
                events[eventName] -= action;
                if (events[eventName] == null)
                {
                    events.Remove(eventName);
                }
            }
        }
    }

    public static class EventManager<T>
    {
        private static readonly Dictionary<string, Action<T>> events = new();

        public static void RaiseEvent(string eventName, T parameter)
        {
            if (events.TryGetValue(eventName, out Action<T> action))
            {
                action?.Invoke(parameter);
            }
        }

        public static void AddListener(string eventName, Action<T> action)
        {
            if (events.ContainsKey(eventName))
            {
                events[eventName] += action;
            }
            else
            {
                events.Add(eventName, action);
            }
        }

        public static void RemoveListener(string eventName, Action<T> action)
        {
            if (events.ContainsKey(eventName))
            {
                events[eventName] -= action;
                if (events[eventName] == null)
                {
                    events.Remove(eventName);
                }
            }
        }
    }
}