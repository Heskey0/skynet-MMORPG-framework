using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 飞行道具
/// </summary>
public class FlyObject : MonoBehaviour
{
    private Transform Target;
    private float Speed;

    private Action<Transform> OnHitTargetCallback;
    Rigidbody rb;
    public void Init(Transform target,float speed,float radius, Action<Transform> hitCallback)
    {
        Target = target;
        Speed = speed;
        OnHitTargetCallback = hitCallback;
        //增加刚体
        rb = gameObject.AddComponent<Rigidbody>();
        //加碰撞器
        gameObject.AddComponent<SphereCollider>().radius = radius;

        rb.useGravity = false;
         
        GameObject.Destroy(gameObject, 3f);
    }

    private void Update()
    {
        rb.velocity = (Target.transform.position - transform.position).normalized * Speed;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (OnHitTargetCallback != null) { OnHitTargetCallback(Target); }
    }
}
