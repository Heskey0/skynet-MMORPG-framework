using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// �������˵����ģ��-->��ɫ�浵
/// </summary>
public class Player
{
    public int ThisID;
    //����ҵĽ�ɫ����
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
