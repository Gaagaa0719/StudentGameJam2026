using System;
using UnityEngine;

public abstract class SingletonObject<T> : MonoBehaviour where T : SingletonObject<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            throw new InvalidOperationException(
                $"{typeof(T).Name} must be a singleton.");
        }

        Instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}