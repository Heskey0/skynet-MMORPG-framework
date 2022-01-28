using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色管理
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
            Debug.LogError("未找到角色模型" + createSceneRoleCmd.ModelID.ToString());
            return;
        }


        Role role;
        //创建角色模型
        var roleObj = ResMgr.Instance.GetInstance(roleDataBase.ModelPath);
        
        //判断是否是主角
        if (Instance._mainRoleThisID == createSceneRoleCmd.ThisID)
        {
            //是主角
            Instance._mainRole = roleObj.AddComponent<MainRole>();
            role = Instance._mainRole;
            Debug.Log("initMainRole");
            SceneMgr.MainCameraController.m_followTarget = role.transform;
        }
        else
        {
            //不是主角
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