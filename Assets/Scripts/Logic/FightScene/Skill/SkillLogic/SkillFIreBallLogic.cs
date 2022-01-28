using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 火球的逻辑
/// </summary>
public class SkillFIreBallLogic
{
    Transform _target;
    private Action _skillEndCallback; //技能结束回调
    Role _caster; //技能施放者
    TimeLine _timeLine; //技能时间线

    public void Init(Role caster)
    {
        _caster = caster;
    }

    private void InitTimeLine()
    {
        //delay id methord
        _timeLine.AddEvent(0, 0, onSkillStart); //技能开始
        _timeLine.AddEvent(0, 10, onAction); //播放动画
        //TODO 播放一个粒子特效
        _timeLine.AddEvent(0.2f, 1, onFlyObject); //生成飞行道具
        //TODO 生成伤害结算物
        _timeLine.AddEvent(1.133f, 10, onActionEnd); //停止动画
        _timeLine.AddEvent(1.133f, 0, onSkillEnd); //技能结束
    }


    private void onSkillEnd(int obj)
    {
        _target = null;
        //通知外界技能已经结束
        if (_skillEndCallback != null)
        {
            _skillEndCallback();
        }
    }

    private void onActionEnd(int actionID)
    {
        if (_caster.GetAnim() == actionID)
        {
            _caster.SetAnim(0);
        }
    }

    private void onFlyObject(int flyObjID)
    {
        var flyDB = FlyObjectTable.Instance[flyObjID];
        if (null == flyDB)
        {
            Debug.LogError("未找到飞行道具" + flyObjID);
            return;
        }

        //放一个火球
        Debug.Log("技能被施放");
        var ball = ResMgr.Instance.GetInstance(flyDB.ResPath);
        var flyObject = ball.AddComponent<FlyObject>();

        flyObject.Init(null, flyDB.FlySpeed, flyDB.Radius, OnHitSomething);
        //火球结束
    }

    private void onAction(int actionID)
    {
        _caster.SetAnim(actionID);
    }

    private void onSkillStart(int id)
    {
    }

    public void Start(Transform target, Action skillEndCallback)
    {
        _target = target;
        _skillEndCallback = skillEndCallback;
    }

    private void OnHitSomething(Transform target)
    {
        Debug.Log("命中目标");
    }

    internal void Loop()
    {
        _timeLine.Loop(Time.deltaTime);
    }
}