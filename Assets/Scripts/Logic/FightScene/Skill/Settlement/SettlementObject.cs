using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// 结算物基类
/// </summary>
public class SettlementObject : MonoBehaviour
{
    private float _radius;
    private Action<List<Creature>> _hitCallback;
    private List<Creature> _hitList;

    public void Init(float radius, Action<List<Creature>> hitCallback)
    {
        _radius = radius;
        _hitCallback = hitCallback;

        var collider = GetComponent<SphereCollider>();
        collider.radius = radius;
        collider.isTrigger = true;
        collider.enabled = true;

        QuickCoroutine.Instance.StartCoroutine(onHitEnd());

    }

    private IEnumerator onHitEnd()
    {
        yield return new WaitForEndOfFrame();
        onEnd();
    }

    /// <summary>
    /// 结算结束
    /// </summary>
    private void onEnd()
    {
        if (_hitCallback!=null)
        {
            _hitCallback(_hitList);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var hitCreature = other.GetComponent<Creature>();
        if (hitCreature == null) { return; }

        _hitList.Add(hitCreature);
    }
}
