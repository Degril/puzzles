using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public abstract class SingletonBase<T>: ISingleton where T : SingletonBase<T> , new() 
{
    protected static T _instance = new T();
    public static T Instance
    {
        get
        {
            _instance.OnGet();
            return _instance;
        }
    }

    protected virtual void OnGet() { }

}

