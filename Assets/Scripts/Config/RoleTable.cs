using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;
/// <summary>
/// ��ɫ�����ݽṹ
/// </summary>
public class RoleDataBase:CreatureDatabase
{

}
/// <summary>
/// ��ɫ��
/// </summary>
public class RoleTable : ConfigTable<RoleDataBase,RoleTable>
{
    //id ������Ŀ
    Dictionary<int, RoleDataBase> _cache = new Dictionary<int, RoleDataBase>();
    public RoleTable()
    {
        load("Config/RoleTable.csv");
    }


}
