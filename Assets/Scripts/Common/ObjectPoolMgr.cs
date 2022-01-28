using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPoolMgr : Singleton<ObjectPoolMgr>
{
    private Transform m_PoolRootObject = null;
    private Dictionary<string, object> m_ObjectPools = new Dictionary<string, object>();
    private Dictionary<string, GameObjectPool> m_GameObjectPools = new Dictionary<string, GameObjectPool>();

    Transform PoolRootObject { get; set; }

    public GameObjectPool CreatePool(string poolName,int initSize,int maxSize,GameObject prefab)
    {
        var pool = new GameObjectPool(poolName,prefab,initSize,maxSize,PoolRootObject);


        return pool;
    }

    public GameObjectPool GetPool(string poolName)
    {
        if (m_GameObjectPools.ContainsKey(poolName))
        {
            return m_GameObjectPools[poolName];
        }
        return null;
    }

    public GameObject Get(string poolName)
    {
        GameObject result = null;
        if (m_GameObjectPools.ContainsKey(poolName))
        {
            GameObjectPool pool = m_GameObjectPools[poolName];
            result = pool.NextAvailableObject();
            if (result==null)
            {
                Debug.LogWarning("对象池空了"+poolName);
            }
        }
        else
        {
            Debug.LogError("没有这个对象池" + poolName);
        }
        return result;
    }

    public void Release()
    {

    }
}

