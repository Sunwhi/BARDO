using System;
using System.Collections.Generic;

public interface IGameEvent { }

public static class GameEventBus
{
    private static Dictionary<Type, List<Delegate>> subscribers = new();

    // ���� ���
    public static void Subscribe<T>(Action<T> callback) where T : IGameEvent
    {
        Type type = typeof(T);
        if (!subscribers.ContainsKey(type))
        {
            subscribers[type] = new List<Delegate>();
        }

        if (!subscribers[type].Contains(callback))
        {
            subscribers[type].Add(callback);
        }
    }

    // ���� ����
    public static void Unsubscribe<T>(Action<T> callback) where T : IGameEvent
    {
        Type type = typeof(T);
        if (subscribers.TryGetValue(type, out var list))
        {
            list.Remove(callback);
        }
    }

    // �̺�Ʈ ����
    public static void Raise<T>(T evt) where T : IGameEvent
    {
        Type type = typeof(T);
        if (subscribers.TryGetValue(type, out var list))
        {
            foreach (var callback in list)
            {
                (callback as Action<T>)?.Invoke(evt);
            }
        }
    }
}