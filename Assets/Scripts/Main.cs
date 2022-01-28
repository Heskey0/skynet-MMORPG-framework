using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

//�˺�123
//����pwd123

public class Main : MonoBehaviour
{
    public bool DebugMode = false;
    
    public enum State
    {
        CheckExtractResource, //����������Ϸʱ��Ҫ��ѹ��Դ�ļ�
        UpdateResourceFromNet, //�ȸ��׶Σ��ӷ��������õ����µ���Դ
        InitAssetBundle, //��ʼ��AssetBundle
        StartLogin, //��¼����
        StartGame, //��ʽ���볡����Ϸ
        Playing, //������������ˣ��������ѿ���Ȩ�����淨�߼�
        None, //��
    }

    public enum SubState
    {
        Enter,
        Update
    }

    State cur_state = State.None;
    SubState cur_sub_state = SubState.Enter;

    private void Start()
    {
        AppConfig.DebugMode = DebugMode;
        
        GameObject.DontDestroyOnLoad(gameObject);
        gameObject.AddComponent<XLuaManager>();
        gameObject.AddComponent<ThreadMgr>();
        gameObject.AddComponent<NetworkManager>();

        GameMgr.Instance.Init(); //GameEngine
        UIManager.Instance.Init();
        QuickCoroutine.Instance.Init();
        XLuaManager.Instance.Init();
        NetMsgDispatcher.Instance.Init();


        JumpToState(State.CheckExtractResource);
    }

    void JumpToState(State new_state)
    {
        cur_state = new_state;
        cur_sub_state = SubState.Enter;
        Debug.Log("new_state : " + new_state.ToString());
    }

    private void Update()
    {
        // ��ѹ��Դ
        // �����£����£������������棬��¼
        if (cur_state == State.Playing)
            return;
        switch (cur_state)
        {
            case State.CheckExtractResource:
                if (cur_sub_state == SubState.Enter)
                {
                    cur_sub_state = SubState.Update;
                    Debug.Log("�״ν�ѹ��Ϸ���ݣ�������������");

                    HotFixManager.Instance.CheckExtractResources(() => { JumpToState(State.UpdateResourceFromNet); });
                }

                break;
            case State.UpdateResourceFromNet:
                if (cur_sub_state == SubState.Enter)
                {
                    cur_sub_state = SubState.Update;
                    Debug.Log("�ӷ������������µ���Դ�ļ�...");

                    HotFixManager.Instance.UpdateABFromNet(() => { JumpToState(State.StartLogin); });
                }

                break;
            case State.StartLogin:
                if (cur_sub_state == SubState.Enter)
                {
                    cur_sub_state = SubState.Update;
                    Debug.Log("��ʼ����Ϸ��Դ���");

                    SceneManager.LoadScene("1.Login");
                    UIManager.Instance.Replace("UI/Login/Login")
                        .GetComponent<Login>().loginCallback += () =>
                    {
                        XLuaManager.Instance.StartLogin(() =>
                        {
                            Debug.Log("login : on_ok");
                            JumpToState(State.StartGame);
                        });
                    };
                }

                break;
            case State.StartGame:
                if (cur_sub_state == SubState.Enter)
                {
                    cur_sub_state = SubState.Update;

                    JumpToState(State.Playing);
                }

                break;
            case State.Playing:
                break;
        }
    }
}