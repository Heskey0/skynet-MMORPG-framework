using System.Collections;
using UnityEngine;


public class SkillDatabase : TableDatabase
{
    //ID
    public string Name;
    public float Desc;
    public float Damage;
    public float CastRange;
    public float PreTime;
    public float CD;
    public float DamageRange;
    public float DuringTime;
    public float Cost;
}

public class SkillTable : ConfigTable<SkillDatabase, SkillTable>
{
    public SkillTable()
    {
        load("Config/SkillTable.csv");
    }
}
