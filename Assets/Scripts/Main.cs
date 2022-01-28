using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

//账号123
//密码pwd123

public class Main : MonoBehaviour
{
    public bool DebugMode = false;
    
    public enum State
    {
        CheckExtractResource, //初次运行游戏时需要解压资源文件
        UpdateResourceFromNet, //热更阶段：从服务器上拿到最新的资源
        InitAssetBundle, //初始化AssetBundle
        StartLogin, //登录流程
        StartGame, //正式进入场景游戏
        Playing, //完成启动流程了，接下来把控制权交给玩法逻辑
        None, //无
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
        // 解压资源
        // 检查更新，更新（重启），公告，登录
        if (cur_state == State.Playing)
            return;
        switch (cur_state)
        {
            case State.CheckExtractResource:
                if (cur_sub_state == SubState.Enter)
                {
                    cur_sub_state = SubState.Update;
                    Debug.Log("首次解压游戏数据（不消耗流量）");

                    HotFixManager.Instance.CheckExtractResources(() => { JumpToState(State.UpdateResourceFromNet); });
                }

                break;
            case State.UpdateResourceFromNet:
                if (cur_sub_state == SubState.Enter)
                {
                    cur_sub_state = SubState.Update;
                    Debug.Log("从服务器下载最新的资源文件...");

                    HotFixManager.Instance.UpdateABFromNet(() => { JumpToState(State.StartLogin); });
                }

                break;
            case State.StartLogin:
                if (cur_sub_state == SubState.Enter)
                {
                    cur_sub_state = SubState.Update;
                    Debug.Log("初始化游戏资源完毕");

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