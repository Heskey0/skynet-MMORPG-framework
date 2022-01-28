using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 服务器端的角色模型
/// </summary>
public class RoleServer
{
    static int _curThisID = 1;
    //生成角色的thisID
    public static int GetNewThisID()
    {
        return ++_curThisID;
    }

    public int ThisID;
    public string Name;//角色名
    public int ModelId;//模型ID
    //攻，防，血
    //角色坐标
    //所在地图
    //道具信息
}
