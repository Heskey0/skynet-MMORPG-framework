using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameObjectPool : MonoBehaviour
{
    //TODO
    private string poolName;
    private GameObject prefab;
    private int initSize;
    private int maxSize;

    private Transform poolRootObject;

    private Stack<GameObject> availableObjStack;

    public GameObjectPool(string poolName, GameObject prefab, int initSize, int maxSize, Transform poolRootObject)
    {
        this.poolName = poolName;
        this.prefab = prefab;
        this.initSize = initSize;
        this.maxSize = maxSize;
        this.poolRootObject = poolRootObject;
    }

    private void AddObjectToPool(GameObject go)
    {
        availableObjStack.Push(go);
    }
    private GameObject NewObjectInstance()
    {
        return null;
    }
    public GameObject NextAvailableObject()
    {
        GameObject go = null;
        go = availableObjStack.Pop();
        return go;
    }

}
