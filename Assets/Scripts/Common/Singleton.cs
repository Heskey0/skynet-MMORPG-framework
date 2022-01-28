using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : class, new()
{
    private static T instance = null;
    private static object LockObj = new object();
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (LockObj)
                {
                    if (instance==null)
                    {
                        instance = new T();
                    }
                }
            }
            return instance;
        }
    }
}
