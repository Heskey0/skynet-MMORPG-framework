using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 本机玩家的用户数据
/// </summary>
public class UserData : Singleton<UserData>
{
    //假的角色数据
    public List<SelectRoleInfo> AllRole = new List<SelectRoleInfo>();
#if DEBUG
    
    public static void OnRoleList(Cmd cmd)
    {
        if (!Net.CheckCmd(cmd,typeof(RoleListCmd)))
        {
            return;
        }
        RoleListCmd roleListCmd = cmd as RoleListCmd;
        
        UserData.Instance.AllRole = roleListCmd.AllRole;
        
        
        if (roleListCmd.AllRole.Count>0)
        {
            //选人界面
            SceneManager.LoadScene("2.SelectRole");
            SceneMgr.Instance.loadScene(2, () =>
            {
                UIManager.Instance.Replace("UI/SelectRole/SelectRole");
            });
            //UIManager.Instance.RemoveLayer();
            //UIManager.Instance._uiRoot.SetActive(false);


        }
        else
        {
            SceneManager.LoadScene("CreateRole");
        }
    }
    
#endif
}

