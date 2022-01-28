using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



/// <summary>
/// ������
/// </summary>
public class Server : Singleton<Server>, IServer
{
    IClient _client;
    IDataBase _db;
    public IDataBase DB => _db;

    /// <summary>
    /// ��Ϣ���� ��Ϣ��������
    /// </summary>
    Dictionary<Type, Action<Cmd>> _parsers = new Dictionary<Type, Action<Cmd>>();

    //��¼���������ϵ����
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

        Debug.Log("�������յ���Ϣ:" + cmd.GetType());
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

