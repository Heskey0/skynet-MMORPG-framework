using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Э�̹�����
/// </summary>
public class QuickCoroutine : Singleton<QuickCoroutine>
{
    GameObject _root;
    MonoBehaviour _coroutineMono;//������Э��
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
