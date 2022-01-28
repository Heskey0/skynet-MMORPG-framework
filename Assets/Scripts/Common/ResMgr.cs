using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源管理器
/// 加载方式 和 使用逻辑分离
/// 以便于支持热更新，对象池
/// </summary>
public class ResMgr : Singleton<ResMgr>
{
    public void Init()
    {
        ObjectPoolMgr.Instance.CreatePool("aa",1,3, GetResources<GameObject>("path"));
    }

    public GameObject GetInstance(string resPath)
    {
        //先从对象池中找
        if (ObjectPoolMgr.Instance.GetPool(resPath) != null)
        {
            return ObjectPoolMgr.Instance.Get(resPath);
        }

        //没有对应的对象池
        return GameObject.Instantiate(GetResources<GameObject>(resPath));
    }

    public T GetResources<T>(string resPath) where T : Object
    {
        return Resources.Load<T>(resPath);
    }

    public void Release(GameObject ui)
    {
        var creature = ui.GetComponent<Creature>();
        if (creature!=null)
        {
            ObjectPoolMgr.Instance.Release();
        }
        GameObject.Destroy(ui);
    }
}
