using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//虚拟协议


/// <summary>
/// 选人界面角色数据模型
/// </summary>
public class SelectRoleInfo
{
    public string Name;//角色名
    public int ModelId;//模型ID
}

/// <summary>
/// 消息基类
/// </summary>
public class Cmd
{

}

/// <summary>
/// 选择角色 C-->S
/// </summary>
public class SelectRoleCmd : Cmd
{
    //角色索引
    public int Index;

}

/// <summary>
/// 登录 C-->S
/// </summary>
public class LoginCmd : Cmd
{
    public string Account;
    public string Password;
}

/// <summary>
/// 玩家的角色列表S-->C
/// </summary>
public class RoleListCmd : Cmd
{
    //保存玩家的所有角色
    public List<SelectRoleInfo> AllRole = new List<SelectRoleInfo>();

}

/// <summary>
/// 主角ThisID   S-->C
/// </summary>
public class MainRoleThisidCmd : Cmd
{
    public int ThisID;
}

public class EnterMapCmd : Cmd
{
    public int MapID;
}


/// <summary>
/// 场景中的角色信息
/// </summary>
public class CreateSceneRoleCmd : Cmd
{
    public int ThisID;    //场景中操控的角色的ID
    public string Name;
    public int ModelID;

    public Vector3 Pos;
    public Vector3 FaceTo;

}

public class CreateSceneNpcCmd : Cmd
{
    public int ThisID;    //场景中操控的角色的ID
    public string Name;
    public int ModelID;

    public Vector3 Pos;
    public Vector3 FaceTo;
}

public class JumpTo : Cmd
{
    public int ID;
}

