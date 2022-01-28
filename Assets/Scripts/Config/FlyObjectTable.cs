using System.Collections;
using UnityEngine;

public class FlyObjectDatabase : TableDatabase
{
    public string Name;
    public string ResPath;
    public float FlySpeed;
    public float Radius;
}

/// <summary>
/// 飞行道具表
/// </summary>
public class FlyObjectTable : ConfigTable<FlyObjectDatabase, FlyObjectTable>
{
    public FlyObjectTable()
    {
        load("Config/FlyTable");
    }

}
