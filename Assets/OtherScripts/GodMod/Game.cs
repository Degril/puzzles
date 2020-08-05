using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class Game
{
    private static Dictionary<string, ISingleton> _utils = new Dictionary<string, ISingleton>();
    
    public static T Get<T>() where T : ISingleton, new()
    {        
        var name = typeof(T).Name;
        if (_utils.TryGetValue(name, out _) == false)
        {
            if (typeof(T).IsSubclassOf(typeof(UtilMonoBehaviour)))
            {
                var obj = new GameObject(name, typeof(T)).GetComponent<T>();
                _utils.Add(name, obj as UtilMonoBehaviour);
            }
            else
            {
                //TODO: edit it later to this return SingletonBase<T>.Instance;                
                switch (typeof(T))
                {
                    case Type data when data == typeof(Data):
                        _utils.Add(name, (Data.Instance));
                        break;
                }
            }
        }
        return (T)Convert.ChangeType(_utils[name], typeof(T));

    }
}
