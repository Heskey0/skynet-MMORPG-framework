using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɫ����
/// </summary>
public class RoleMgr : Singleton<RoleMgr>
{
    int _mainRoleThisID;
    private MainRole _mainRole;

    public MainRole MainRole
    {
        get { return _mainRole; }
    }

    //thisID , Role
    public Dictionary<int, Role> AllRole = new Dictionary<int, Role>();

    internal static void OnMainRoleThisid(Cmd cmd)
    {
        if (!Net.CheckCmd(cmd, typeof(MainRoleThisidCmd)))
        {
            return;
        }
        MainRoleThisidCmd mainRoleThisidCmd = cmd as MainRoleThisidCmd;
        
        RoleMgr.Instance._mainRoleThisID = mainRoleThisidCmd.ThisID;
    }

    internal static void OnCreateSceneRole(Cmd cmd)
    {
        //create scene role
        if (!Net.CheckCmd(cmd, typeof(CreateSceneRoleCmd)))
        {
            return;
        }
        CreateSceneRoleCmd createSceneRoleCmd = cmd as CreateSceneRoleCmd;

        RoleDataBase roleDataBase = RoleTable.Instance[createSceneRoleCmd.ModelID];
        if (roleDataBase == null)
        {
            Debug.LogError("δ�ҵ���ɫģ��" + createSceneRoleCmd.ModelID.ToString());
            return;
        }


        Role role;
        //������ɫģ��
        var roleObj = ResMgr.Instance.GetInstance(roleDataBase.ModelPath);
        
        //�ж��Ƿ�������
        if (Instance._mainRoleThisID == createSceneRoleCmd.ThisID)
        {
            //������
            Instance._mainRole = roleObj.AddComponent<MainRole>();
            role = Instance._mainRole;
            Debug.Log("initMainRole");
            SceneMgr.MainCameraController.m_followTarget = role.transform;
        }
        else
        {
            //��������
            role = roleObj.AddComponent<Role>();
        }

        role.Init(createSceneRoleCmd, roleDataBase);

        RoleMgr.Instance.AllRole[createSceneRoleCmd.ThisID] = role;
    }

    public void Reset()
    {
        foreach (var role in AllRole)
        {
            ResMgr.Instance.Release(role.Value.gameObject);
        }
        RoleMgr.Instance.AllRole.Clear();
    }
}