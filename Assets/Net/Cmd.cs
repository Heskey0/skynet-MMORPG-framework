using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����Э��


/// <summary>
/// ѡ�˽����ɫ����ģ��
/// </summary>
public class SelectRoleInfo
{
    public string Name;//��ɫ��
    public int ModelId;//ģ��ID
}

/// <summary>
/// ��Ϣ����
/// </summary>
public class Cmd
{

}

/// <summary>
/// ѡ���ɫ C-->S
/// </summary>
public class SelectRoleCmd : Cmd
{
    //��ɫ����
    public int Index;

}

/// <summary>
/// ��¼ C-->S
/// </summary>
public class LoginCmd : Cmd
{
    public string Account;
    public string Password;
}

/// <summary>
/// ��ҵĽ�ɫ�б�S-->C
/// </summary>
public class RoleListCmd : Cmd
{
    //������ҵ����н�ɫ
    public List<SelectRoleInfo> AllRole = new List<SelectRoleInfo>();

}

/// <summary>
/// ����ThisID   S-->C
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
/// �����еĽ�ɫ��Ϣ
/// </summary>
public class CreateSceneRoleCmd : Cmd
{
    public int ThisID;    //�����вٿصĽ�ɫ��ID
    public string Name;
    public int ModelID;

    public Vector3 Pos;
    public Vector3 FaceTo;

}

public class CreateSceneNpcCmd : Cmd
{
    public int ThisID;    //�����вٿصĽ�ɫ��ID
    public string Name;
    public int ModelID;

    public Vector3 Pos;
    public Vector3 FaceTo;
}

public class JumpTo : Cmd
{
    public int ID;
}

