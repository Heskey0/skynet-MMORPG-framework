using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;

/// <summary>
/// Npc表数据结构
/// </summary>
public class NpcDataBase: TableDatabase
{
    public string Name;
    public string ModelPath;
}
/// <summary>
/// Npc表
/// </summary>
public class NpcTable : ConfigTable<NpcDataBase,NpcTable>
{
    
    //id 数据条目
    Dictionary<int, NpcDataBase> _cache = new Dictionary<int, NpcDataBase>();

    public NpcTable()
    {
        load("Config/NpcTable.csv");
    }





}
