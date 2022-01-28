using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FightUIMgr :Singleton<FightUIMgr>
{
    //遥杆
    private Joystick _joystick;
    //与场景的点击交互
    private TouchScene _touchScene;
    //技能按钮
    //人物头像
    //小地图
    //目标头像
    //功能大厅
    private FunctionHall _functionHall;
    
    private SkillAtkDlg _skillAtkDlg;

    public void Init()
    {
        if (_joystick==null)
        {
            _joystick = new Joystick();
        }

        if (_touchScene==null)
        {
            _touchScene= new TouchScene();
        }

        if (_functionHall ==null)
        {
            _functionHall = new FunctionHall();
        }
    }

    /// <summary>
    /// 绑定事件
    /// </summary>
    /// <param name="onJoystickMove"></param>
    /// <param name="onJoystickMoveEnd"></param>
    public void BindingJoyStick(Action<Vector2> onJoystickMove,Action onJoystickMoveEnd)
    {
        if (_joystick==null)
        {
            return;
        }
        _joystick.OnMoveDir = onJoystickMove;
        _joystick.OnMoveEnd = onJoystickMoveEnd;
    }
    public void BindingTouchScene(Action<RaycastHit> onTouchSth,int layer)
    {
        if (_touchScene==null)
        {
            return;
        }
        _touchScene.UnTouchLayer = layer;
        _touchScene.HitSthCallback = onTouchSth;
    }
    public void BindingSkillBtn(Action<int> skillBtnCallback)
    {
        if (_skillAtkDlg==null)
        {
            return;
        }
        _skillAtkDlg.OnSkillBtnClick = skillBtnCallback;
    }
    
    /// <summary>
    /// 释放事件
    /// </summary>
    public void ReleaseJoyStick()
    {     
        if (_joystick==null)
        {
            return;
        }
        _joystick.OnMoveDir = null;
        _joystick.OnMoveEnd = null;
    }
    public void ReleaseTouchScene()
    {
        if (_touchScene==null)
        {
            return;
        }
        _touchScene.HitSthCallback = null;
    }
    public void ReleaseSkillBtn()
    {
        if (_skillAtkDlg==null)
        {
            return;
        }
        _skillAtkDlg.OnSkillBtnClick = null;
    }


    private void resetJoystick()
    {
        if (_joystick==null)
        {
            return;
        }
        _joystick.Reset();
    }
    public void Reset()
    {
        ReleaseJoyStick();
        ReleaseTouchScene();
        ReleaseSkillBtn();

        resetJoystick();
    }


    
}
