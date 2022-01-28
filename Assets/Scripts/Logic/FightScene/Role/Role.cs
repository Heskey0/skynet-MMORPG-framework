using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 角色
/// </summary>
public class Role : Creature
{
    private SkillMgr _skillMgr;

    //创建角色的数据
    CreateSceneRoleCmd _serverData;
    RoleDataBase _tableData;

    public int ThisID => _serverData.ThisID;
    public string ModelPath => _tableData.ModelPath;

    protected NavMeshAgent _agent;
    Animator _animator;

    //private Vector3? _targetPoint = null;

    //角色当前的目的
    private Purpose _purpose;

    public virtual void Init(CreateSceneRoleCmd serverData, RoleDataBase tableData)
    {
        _serverData = serverData;
        _tableData = tableData;

        transform.position = _serverData.Pos;
        transform.LookAt(_serverData.FaceTo);

        _agent = gameObject.AddComponent<NavMeshAgent>();
        _agent.stoppingDistance = GameSettings.StopDistance;
        _agent.speed = 5f;
        _agent.angularSpeed = float.MaxValue;
        _agent.acceleration = float.MaxValue;

        _animator = gameObject.GetComponent<Animator>();
    }

    public void SetAnim(int motionType)
    {
        _animator.SetInteger("MotionType", motionType);
    }

    public int GetAnim()
    {
        return _animator.GetInteger("MotionType");
    }

    private void pathTo(Vector3 target)
    {
        _agent.SetDestination(target);
        SetAnim(1);
        //_targetPoint = target;
    }

    public void JoystickMove(Vector3 target)
    {
        resetPurpose();
        PurposeTo(target);
    }

    public void TouchGroundPathTo(Vector3 target)
    {
        resetPurpose();
        PurposeTo(target);
    }

    private void resetPurpose()
    {
        _purpose = null;
    }

    /// <summary>
    /// 移动到目标附近并触发事件
    /// </summary>
    /// <param name="target"></param>
    /// <param name="stopDistance"></param>
    /// <param name="arrivedCallback"></param>
    public void PurposeTo(Vector3 target, float stopDistance = -1, Action arrivedCallback = null)
    {
        if (stopDistance < 0)
        {
            stopDistance = GameSettings.StopDistance;
        }
        
        resetPurpose();
        
        if (Util.Distance2_5D(transform.position,target) < stopDistance)
        {
            if (arrivedCallback!=null)
            {
                arrivedCallback();
            }
            return;
        }
        
        _purpose = new Purpose();
        _purpose.TargetPos = target;
        _purpose.StopDistance = stopDistance;
        _purpose.Callback = arrivedCallback;
        
        pathTo(target);
    }

    public void StopMove()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
        SetAnim(0);

        //_targetPoint = null;
        resetPurpose();
    }

    private void Update()
    {
        //Physics.BoxCast
        if (_purpose == null)
        {
            return;
        }


        var dis = Util.Distance2_5D(transform.position, _purpose.TargetPos);


        if (dis < _purpose.StopDistance)
        {
            onArrived();
        }
    }

    private void onArrived()
    {
        doPurpose();
        StopMove();
    }

    private void doPurpose()
    {
        if (_purpose != null)
        {
            if (_purpose.Callback != null)
            {
                _purpose.Callback();
                _purpose = null;
            }
        }
    }
}