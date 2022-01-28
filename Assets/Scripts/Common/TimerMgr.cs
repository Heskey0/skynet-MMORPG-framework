using System;
using UnityEngine;

/// <summary>
/// �������еĶ�ʱ��
/// </summary>
public class  TimerMgr : Singleton<TimerMgr>
{
    public event Action<float> TimerLoopCallBack;

    /// <summary>
    /// ������ʱ��
    /// </summary>
    /// <param name="deltaTime">ÿ��ִ�м��</param>
    /// <param name="repeatTimes">ִ�д���</param>
    /// <param name="callBack">ÿ��ִ����ϵĻص�</param>
    /// <returns></returns>
    public Timer CreateTimer(float deltaTime, int repeatTimes, Action callBack)
    {
        var timer = new Timer();
        timer.DeltaTime = deltaTime;
        timer.RepeatTimes = repeatTimes;
        timer.CallBack = callBack;


        //�����ί��ִ��ʱɾ���ڲ�����������ί��ִ�����ʱ�Ż�ɾ��
        //_timerLoopCallBack -= timer.CallBack;
        return timer;
    }

    public void Loop(float deltaTime)
    {
        if (TimerLoopCallBack != null) { TimerLoopCallBack(deltaTime); }
    }


}

/// <summary>
/// ��ʱ��
/// </summary>
public class Timer
{
    public float DeltaTime;
    public int RepeatTimes;
    public Action CallBack;
    private int repeatTimes = 0; //�Ѿ�ִ�еĴ���
    private float _passedTime = 0;//��ʱ�ĳ���ʱ��

    public bool IsRunning = false;//�Ƿ�����ִ��

    /// <summary>
    /// ���ö�ʱ��
    /// </summary>
    public void ReSet()
    {
        repeatTimes = 0;
        _passedTime = 0;
        IsRunning = false;
    }



    //��ʼ ��ͣ Loop�ڼ�ʱ
    /// <summary>
    /// ����ʱ����ӵ�ִ���б�
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
    /// ��ͣ������
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
            //���ܹ�ִ�д���Ϊ������ʱ�� ����ѭ��
            if (RepeatTimes < 0) return;
            ++repeatTimes;
            if (repeatTimes == RepeatTimes) { Stop(); }
        }
    }
}