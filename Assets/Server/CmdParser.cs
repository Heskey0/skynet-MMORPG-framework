using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// <summary>
/// �������˵���Ϣ������
/// </summary>
public static class CmdParser
{
    /// <summary>
    /// ��¼����
    /// </summary>
    /// <param name="cmd"></param>
    public static void OnLogin(Cmd cmd)
    {
        if (!Net.CheckCmd(cmd, typeof(LoginCmd)))
        {
            return;
        }
        LoginCmd loginCmd = cmd as LoginCmd;


        //��֤�˺�����
        //�ҵ���ҵĴ浵
        var playerData = Server.Instance.DB.GetUserData(1);    //�����ݿ�ȡ�浵
        if (playerData==null)
        {
            playerData = new Player();
            playerData.ThisID = 1;                //������ҵ�thisID
            Server.Instance.DB.SavePlayerData(playerData);
        }

        Server.Instance.CurPlayer = playerData;
        var player = Server.Instance.CurPlayer;

        //��ͻ��˷�������Ѿ�������ɫ���б�
        RoleListCmd roleListCmd = new RoleListCmd();
        foreach (var role in player.AllRole)
        {
            var roleInfo = new SelectRoleInfo() {Name = role.Name, ModelId = role.ModelId};
            roleListCmd.AllRole.Add(roleInfo);
        }

        Server.Instance.SendCmd(roleListCmd);
        Debug.Log("Login");
    }

    /// <summary>
    /// ���볡������
    /// </summary>
    /// <param name="cmd"></param>
    public static void OnSelectRole(Cmd cmd)
    {
        //Debug.Log("on select role");
        if (!Net.CheckCmd(cmd, typeof(SelectRoleCmd)))
        {
            return;
        }
        
        SelectRoleCmd selectRoleCmd = cmd as SelectRoleCmd;

        var curPlayer = Server.Instance.CurPlayer;
        var curRole = curPlayer.AllRole[selectRoleCmd.Index];
        curPlayer.CurRole = curRole;
        curPlayer.CurRole.ThisID = RoleServer.GetNewThisID();
        //���볡��
        enterMap(curRole,1);
    }

    private static void enterMap(RoleServer curRole,int sceneID)
    {


        //���߿ͻ��� �������
        //var sceneID = 1;
        EnterMapCmd enterMapCmd = new EnterMapCmd() {MapID = sceneID};
        //����ThisID
        var thisid = curRole.ThisID;
        //���߿ͻ��� ���ǵ�ThisID
        MainRoleThisidCmd thisidCmd = new MainRoleThisidCmd();
        thisidCmd.ThisID = thisid;
        
        CreateSceneRoleCmd roleCmd = new CreateSceneRoleCmd();
        roleCmd.ThisID = thisid;
        roleCmd.Name = curRole.Name;
        roleCmd.ModelID = curRole.ModelId;
        roleCmd.Pos = Vector3.zero;
        roleCmd.FaceTo = Vector3.forward;

        Server.Instance.SendCmd(enterMapCmd);    //���߿ͻ��� �������
        Server.Instance.SendCmd(thisidCmd);      //���߿ͻ��� ���ǵ�ThisID
        Server.Instance.SendCmd(roleCmd);        //���߿ͻ��� ��������

        //���ɸ�������ǣ������ǣ�
        //���ɸ�����NPC
        CreateSomeNpc();
    }

    private static void CreateSomeNpc()
    {
        var npc1Cmd = new CreateSceneNpcCmd();
        npc1Cmd.ThisID = RoleServer.GetNewThisID();
        npc1Cmd.ModelID = 1;
        npc1Cmd.Name = NpcTable.Instance[npc1Cmd.ModelID].Name;
        npc1Cmd.Pos=new Vector3(0,-0.8f,2);
        
        var npc2Cmd = new CreateSceneNpcCmd();
        npc2Cmd.ThisID = RoleServer.GetNewThisID();
        npc2Cmd.ModelID = 2;
        npc2Cmd.Name = NpcTable.Instance[npc2Cmd.ModelID].Name;
        npc2Cmd.Pos=new Vector3(2,0,2);

        var npc3Cmd = new CreateSceneNpcCmd();
        npc3Cmd.ThisID = RoleServer.GetNewThisID();
        npc3Cmd.ModelID = 3;
        npc3Cmd.Name = NpcTable.Instance[npc3Cmd.ModelID].Name;
        npc3Cmd.Pos=new Vector3(2,0,0);
        
        Server.Instance.SendCmd(npc1Cmd);
        Server.Instance.SendCmd(npc2Cmd);
        Server.Instance.SendCmd(npc3Cmd);
    }

    public static void OnJumpMap(Cmd cmd)
    {
        if (!Net.CheckCmd(cmd, typeof(JumpTo)))
        {
            return;
        }
        JumpTo jumpToCmd = cmd as JumpTo;
        
        //��֤������Ϣ
        
        //����ͼ
        enterMap(Server.Instance.CurPlayer.CurRole,jumpToCmd.ID);
    }
}

