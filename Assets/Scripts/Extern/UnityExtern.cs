using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtern
{
    public static T Find<T>(this GameObject parent,string path)where T:class
    {
        var targetObj = parent.transform.Find(path);
        if (targetObj == null) return null;
        return targetObj.GetComponent<T>();
    }

    public static void DestroyAllChildren(this GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject.Destroy(parent.transform.GetChild(i).gameObject);
        }
    }
}
