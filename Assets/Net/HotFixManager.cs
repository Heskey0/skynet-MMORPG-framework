//code by 赫斯基皇
//https://space.bilibili.com/455965619
//https://github.com/Heskey0

using System;
using UnityEngine;

public class HotFixManager : Singleton<HotFixManager>
{
    /// <summary>
    /// 检查资源是否解压
    /// </summary>
    public void CheckExtractResources(Action on_ok)
    {
        //TODO:检查资源是否解压
        on_ok?.Invoke();
    }

    public void UpdateABFromNet(Action on_ok)
    {
        //TODO:从服务器下载AB包
        on_ok?.Invoke();
    }
}
