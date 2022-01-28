//code by 赫斯基皇
//https://space.bilibili.com/455965619
//https://github.com/Heskey0

using System;
using System.Collections.Generic;
using UnityEngine;

public class NpcMgr : Singleton<NpcMgr>
{
    public Dictionary<int, Npc> AllNpc = new Dictionary<int, Npc>();

    public static void OnCreateSceneNpc(Cmd cmd)
    {
        if (!Net.CheckCmd(cmd, typeof(CreateSceneNpcCmd)))
        {
            return;
        }
        CreateSceneNpcCmd createNpc = cmd as CreateSceneNpcCmd;

        var npcDatabase = NpcTable.Instance[createNpc.ModelID];
        var npcObj = ResMgr.Instance.GetInstance(npcDatabase.ModelPath);
        var npc = npcObj.AddComponent<Npc>();

        npc.Init(createNpc, npcDatabase);
    }

    public void Reset()
    {
        foreach (var npc in AllNpc)
        {
            ResMgr.Instance.Release(npc.Value.gameObject);
        }
        NpcMgr.Instance.AllNpc.Clear();
    }
}