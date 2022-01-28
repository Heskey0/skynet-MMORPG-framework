using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;
/// <summary>
/// 角色表数据结构
/// </summary>
public class RoleDataBase:CreatureDatabase
{

}
/// <summary>
/// 角色表
/// </summary>
public class RoleTable : ConfigTable<RoleDataBase,RoleTable>
{
    //id 数据条目
    Dictionary<int, RoleDataBase> _cache = new Dictionary<int, RoleDataBase>();
    public RoleTable()
    {
        load("Config/RoleTable.csv");
    }


}
