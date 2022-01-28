using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 服务器端的玩家模型-->角色存档
/// </summary>
public class Player
{
    public int ThisID;
    //该玩家的角色数据
    public List<RoleServer> AllRole = new List<RoleServer>();

    public RoleServer CurRole;
    public Player()
    {

        // AllRole.Add(new RoleServer()
        // {
        //     Name = "Sphere",
        //     ModelId = 1
        // });
        // AllRole.Add(new RoleServer()
        // {
        //     Name = "Capsule",
        //     ModelId = 2
        // });
        // AllRole.Add(new RoleServer()
        // {
        //     Name = "Cube",
        //     ModelId = 3
        // });
    }
}
