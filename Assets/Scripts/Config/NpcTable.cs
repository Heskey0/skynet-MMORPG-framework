using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;

/// <summary>
/// Npc�����ݽṹ
/// </summary>
public class NpcDataBase: TableDatabase
{
    public string Name;
    public string ModelPath;
}
/// <summary>
/// Npc��
/// </summary>
public class NpcTable : ConfigTable<NpcDataBase,NpcTable>
{
    
    //id ������Ŀ
    Dictionary<int, NpcDataBase> _cache = new Dictionary<int, NpcDataBase>();

    public NpcTable()
    {
        load("Config/NpcTable.csv");
    }





}
