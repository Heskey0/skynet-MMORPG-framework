using System;
using UnityEngine;

/// <summary>
/// 管理所有的定时器
/// </summary>
public class  TimerMgr : Singleton<TimerMgr>
{
    public event Action<float> TimerLoopCallBack;

    /// <summary>
    /// 创建定时器
    /// </summary>
    /// <param name="deltaTime">每次执行间隔</param>
    /// <param name="repeatTimes">执行次数</param>
    /// <param name="callBack">每次执行完毕的回调</param>
    /// <returns></returns>
    public Timer CreateTimer(float deltaTime, int repeatTimes, Action callBack)
    {
        var timer = new Timer();
        timer.DeltaTime = deltaTime;
        timer.RepeatTimes = repeatTimes;
        timer.CallBack = callBack;


        //如果在委托执行时删除内部方法，会在委托执行完毕时才会删除
        //_timerLoopCallBack -= timer.CallBack;
        return timer;
    }

    public void Loop(float deltaTime)
    {
        if (TimerLoopCallBack != null) { TimerLoopCallBack(deltaTime); }
    }


}

/// <summary>
/// 定时器
/// </summary>
public class Timer
{
    public float DeltaTime;
    public int RepeatTimes;
    public Action CallBack;
    private int repeatTimes = 0; //已经执行的次数
    private float _passedTime = 0;//计时的持续时间

    public bool IsRunning = false;//是否正在执行

    /// <summary>
    /// 重置定时器
    /// </summary>
    public void ReSet()
    {
        repeatTimes = 0;
        _passedTime = 0;
        IsRunning = false;
    }



    //开始 暂停 Loop内计时
    /// <summary>
    /// 将定时器添加到执行列表
    /// </summary>
    public void Start()
    {
        ReSet();
        IsRunning = true;
        TimerMgr.Instance.TimerLoopCallBack += Loop;
    }
    public void Pause()
    {
        IsRunning = false;
        TimerMgr.Instance.TimerLoopCallBack -= Loop;
    }
    /// <summary>
    /// 暂停并重置
    /// </summary>
    public void Stop()
    {
        Pause();
        ReSet();
    }


    public void Loop(float deltaTime)
    {
        _passedTime += deltaTime;
        if (_passedTime > DeltaTime || Util.Equals(_passedTime,DeltaTime))
        {
            _passedTime -= DeltaTime;

            if (CallBack != null) { CallBack(); }
            //当总共执行次数为负数的时候 无限循环
            if (RepeatTimes < 0) return;
            ++repeatTimes;
            if (repeatTimes == RepeatTimes) { Stop(); }
        }
    }
}