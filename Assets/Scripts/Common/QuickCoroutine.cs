using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 协程管理器
/// </summary>
public class QuickCoroutine : Singleton<QuickCoroutine>
{
    GameObject _root;
    MonoBehaviour _coroutineMono;//用来跑协程
    public void Init()
    {
        _root = new GameObject("QuickCoroutine");
        GameObject.DontDestroyOnLoad(_root);
        _coroutineMono = _root.AddComponent<CoroutineMono>();
    }
    


    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return _coroutineMono.StartCoroutine(routine);
        
    }
}

public class CoroutineMono : MonoBehaviour
{

}
