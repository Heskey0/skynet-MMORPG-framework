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
/// ����ͻ���
/// </summary>
public class Net : Singleton<Net>, IClient
{
    IServer _server;

    /// <summary>
    /// ��Ϣ���� ��Ϣ��������
    /// </summary>
    Dictionary<Type, Action<Cmd>> _parser = new Dictionary<Type, Action<Cmd>>();

    //��Ϣ�Ļ���
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
        //��_Server��ֵ
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
            //����ʧ��
            if (failedCallback != null)
            {
                failedCallback(null);
            }
        }
    }

    public void Receive(Cmd cmd)
    {
        Debug.Log("�ͻ����յ���Ϣ");
        if (cmd != null)
            _cache.Add(cmd);
        //������Ϣ
        if (Pause)
        {
            return;
        }

        //�ַ���Ϣ
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
    /// <returns>�Ƿ���Ŀ��Cmd����</returns>
    public static bool CheckCmd(Cmd cmd, Type targetType)
    {
        if (cmd.GetType() != targetType)
        {
            Debug.LogError(string.Format("��Ҫ{0}�������յ���{1}", targetType, cmd.GetType()));
            return false;
        }

        return true;
    }

    #endregion
}

