//code by 赫斯基皇
//https://space.bilibili.com/455965619
//https://github.com/Heskey0

using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 摇杆
/// </summary>
public class Joystick
{
    public Action<Vector2> OnMoveDir; //有输入时的回调
    public Action OnMoveEnd; //无输入时的回调

    private GameObject _root;
    private RectTransform _innerBall; //内部小球
    private RectTransform _outBall; //外部小球

    private float _radius = 0;
    private Vector2 _centerPos; //小球相对于大球的中心点

    private Vector2 _dir; //实际移动方向

    public Joystick()
    {
        _root = UIManager.Instance.Add("UI/FightUI/JoyStick", UILayer.FightUI);

        _outBall = _root.Find<RectTransform>("bg");
        _innerBall = _root.Find<RectTransform>("bg/inner");

        // 小球的移动半径
        _radius = _outBall.rect.width * 0.5f;
        // 大球的中心店
        _centerPos = new Vector2(0, 0); //_innerBall.localPosition;

        var touchEx = _outBall.GetComponent<TouchEx>();
        touchEx.DragCallBack = onDrag;
        touchEx.PointerDownCallBack = onPointerDown;
        touchEx.PointerUpCallBack = onPointerUp;

        TimerMgr.Instance.CreateTimer(0, -1, onLoop).Start();
    }

    /// <summary>
    /// _dir不为0时调用移动函数
    /// </summary>
    private void onLoop()
    {
        if (_dir==Vector2.zero)
        {
            return;
        }
        if (OnMoveDir != null)
        {
            OnMoveDir(_dir);
        }
    }

    /// <summary>
    /// 拖拽时更新_dir
    /// </summary>
    /// <param name="eventData"></param>
    private void onDrag(PointerEventData eventData)
    {
        var worldCenterPos = _outBall.TransformPoint(_centerPos); //相对于大球的中心坐标-->世界坐标

        var touchPos = eventData.position;
        var dirV = touchPos - new Vector2(worldCenterPos.x, worldCenterPos.y);
        var dir = dirV.normalized;
        _dir = dir;

        _innerBall.position = new Vector2(worldCenterPos.x,worldCenterPos.y) + dir * Mathf.Min(_radius, Vector2.Distance(touchPos, worldCenterPos));
        
    }

    private void onPointerDown(PointerEventData eventData)
    {
        onDrag(eventData);
    }

    private void onPointerUp(PointerEventData eventData)
    {
        Reset();
        
        if (OnMoveEnd != null)
        {
            OnMoveEnd();
        }
    }

    public void Reset()
    {
        _dir = Vector2.zero;
        var worldCenterPos = _outBall.TransformPoint(_centerPos);
        _innerBall.position = new Vector2(worldCenterPos.x, worldCenterPos.y);
    }
}