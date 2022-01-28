using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



/// <summary>
/// 服务器
/// </summary>
public class Server : Singleton<Server>, IServer
{
    IClient _client;
    IDataBase _db;
    public IDataBase DB => _db;

    /// <summary>
    /// 消息类型 消息解析函数
    /// </summary>
    Dictionary<Type, Action<Cmd>> _parsers = new Dictionary<Type, Action<Cmd>>();

    //登录到服务器上的玩家
    public Player CurPlayer;

    public Server()
    {
        _db = SQLiteMgr.Instance;
        _db.Init();

        _parsers.Add(typeof(LoginCmd), CmdParser.OnLogin);
        _parsers.Add(typeof(SelectRoleCmd), CmdParser.OnSelectRole);
        _parsers.Add(typeof(JumpTo), CmdParser.OnJumpMap);
        
        
    }
    public void Connect(IClient client)
    {
        _client = client;
    }

    public void Receive(Cmd cmd)
    {

        Debug.Log("服务器收到消息:" + cmd.GetType());
        Action<Cmd> func;
        if (_parsers.TryGetValue(cmd.GetType(), out func))
        {
            if (func != null) { func(cmd); }
        }
    }

    public void SendCmd(Cmd cmd)
    {
        _client.Receive(cmd);
    }
}

