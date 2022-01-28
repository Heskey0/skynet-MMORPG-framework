using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapDataBase:TableDatabase
{
    //int ID
    public string Name;
    /// <summary>
    /// ��������
    /// </summary>
    public string ScenePath;
}

public class MapTable : ConfigTable<MapDataBase, MapTable>
{
    public MapTable()
    {
        load("Config/MapTable.csv");
    }
}
