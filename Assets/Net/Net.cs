using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public interface IClient
{
    void SendCmd(Cmd cmd);
    void Receive(Cmd cmd);
}

public interface IServer
{
    void Connect(IClient client);
    void SendCmd(Cmd cmd);
    void Receive(Cmd cmd);
}

/// <summary>
/// 虚拟客户端
/// </summary>
public class Net : Singleton<Net>, IClient
{
    IServer _server;

    /// <summary>
    /// 消息类型 消息解析函数
    /// </summary>
    Dictionary<Type, Action<Cmd>> _parser = new Dictionary<Type, Action<Cmd>>();

    //消息的缓存
    List<Cmd> _cache = new List<Cmd>();

    public Net()
    {
        _parser.Add(typeof(RoleListCmd), UserData.OnRoleList);
        _parser.Add(typeof(EnterMapCmd), SceneMgr.OnEnterMap);
        _parser.Add(typeof(MainRoleThisidCmd), RoleMgr.OnMainRoleThisid);
        _parser.Add(typeof(CreateSceneRoleCmd), RoleMgr.OnCreateSceneRole);
        _parser.Add(typeof(CreateSceneNpcCmd),NpcMgr.OnCreateSceneNpc);
    }

    private bool _pause;

    public bool Pause
    {
        get { return _pause; }
        set
        {
            _pause = value;
            if (_pause == false)
            {
                Receive(null);
            }
        }
    }

    public void ConnectServer(Action<byte[]> successCallback, Action<byte[]> failedCallback)
    {
        //给_Server赋值
        _server = Server.Instance;
        _server.Connect(this);

        if (true)
        {
            if (successCallback != null)
            {
                successCallback(null);
            }
        }
        else
        {
            //连接失败
            if (failedCallback != null)
            {
                failedCallback(null);
            }
        }
    }

    public void Receive(Cmd cmd)
    {
        Debug.Log("客户端收到消息");
        if (cmd != null)
            _cache.Add(cmd);
        //阻塞消息
        if (Pause)
        {
            return;
        }

        //分发消息
        foreach (var cacheCmd in _cache)
        {
            //_server.Send();
            Action<Cmd> func;
            if (_parser.TryGetValue(cacheCmd.GetType(), out func))
            {
                if (func != null)
                {
                    func(cacheCmd);
                }
            }
        }

        _cache.Clear();
    }

    public void SendCmd(Cmd cmd)
    {
        _server.Receive(cmd);
    }

    #region other

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="targetType"></param>
    /// <returns>是否是目标Cmd类型</returns>
    public static bool CheckCmd(Cmd cmd, Type targetType)
    {
        if (cmd.GetType() != targetType)
        {
            Debug.LogError(string.Format("需要{0}，但是收到了{1}", targetType, cmd.GetType()));
            return false;
        }

        return true;
    }

    #endregion
}

