//code by 赫斯基皇
//https://space.bilibili.com/455965619
//https://github.com/Heskey0

using System;
using UnityEngine;


public static class AppConfig
{
    //调试模式
    public static bool DebugMode =false;

    public static string LuaAssetsDir
    {
        get { return Application.dataPath + "/../Lua"; }
    }

    public static string ServerIp = "192.168.3.70";
    public static int Port = 8888;
}