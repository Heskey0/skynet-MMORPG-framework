using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������߼�
/// </summary>
public class SkillFIreBallLogic
{
    Transform _target;
    private Action _skillEndCallback; //���ܽ����ص�
    Role _caster; //����ʩ����
    TimeLine _timeLine; //����ʱ����

    public void Init(Role caster)
    {
        _caster = caster;
    }

    private void InitTimeLine()
    {
        //delay id methord
        _timeLine.AddEvent(0, 0, onSkillStart); //���ܿ�ʼ
        _timeLine.AddEvent(0, 10, onAction); //���Ŷ���
        //TODO ����һ��������Ч
        _timeLine.AddEvent(0.2f, 1, onFlyObject); //���ɷ��е���
        //TODO �����˺�������
        _timeLine.AddEvent(1.133f, 10, onActionEnd); //ֹͣ����
        _timeLine.AddEvent(1.133f, 0, onSkillEnd); //���ܽ���
    }


    private void onSkillEnd(int obj)
    {
        _target = null;
        //֪ͨ��缼���Ѿ�����
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
            Debug.LogError("δ�ҵ����е���" + flyObjID);
            return;
        }

        //��һ������
        Debug.Log("���ܱ�ʩ��");
        var ball = ResMgr.Instance.GetInstance(flyDB.ResPath);
        var flyObject = ball.AddComponent<FlyObject>();

        flyObject.Init(null, flyDB.FlySpeed, flyDB.Radius, OnHitSomething);
        //�������
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
        Debug.Log("����Ŀ��");
    }

    internal void Loop()
    {
        _timeLine.Loop(Time.deltaTime);
    }
}