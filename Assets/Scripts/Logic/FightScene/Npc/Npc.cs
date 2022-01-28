//code by 赫斯基皇
//https://space.bilibili.com/455965619
//https://github.com/Heskey0

using System;
using UnityEngine;

public class Npc : MonoBehaviour
{
    CreateSceneNpcCmd _serverData;
    NpcDataBase _tableData;

    public int ThisID => _serverData.ThisID;
    public string Name => _serverData.Name;    //优先使用服务器的数据
    public void Init(CreateSceneNpcCmd cmd, NpcDataBase dataBase)
    {
        _serverData = cmd;
        _tableData = dataBase;

        transform.position = _serverData.Pos;
        transform.eulerAngles = new Vector3(0,180,0);
    }
}