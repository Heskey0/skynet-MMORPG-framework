//code by 赫斯基皇
//https://space.bilibili.com/455965619
//https://github.com/Heskey0

using System;
using System.IO;
using UnityEngine;
using XLua;



public class Param
{
    
}

[Hotfix]
[LuaCallCSharp]
public class XLuaManager : MonoBehaviour
{

    public static XLuaManager Instance = null;
    
    private LuaEnv luaEnv = null;
    private Action<string, string> luaUpdate = null;
    private Action<string> luaFixedUpdate = null;
    private Action luaLateUpdate = null;
    
    private Action onLoginOk = null;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public void Init()
    {
        luaEnv = new LuaEnv();
        luaEnv.AddLoader(CustomLoader);
        LoadScript("Init");
        luaUpdate = luaEnv.Global.Get<Action<string, string>>("Update");
        luaLateUpdate = luaEnv.Global.Get<Action>("LateUpdate");
        luaFixedUpdate = luaEnv.Global.Get<Action<string>>("FixedUpdate");

        // Debug.Log(luaUpdate!=null);
        // Debug.Log(luaLateUpdate!=null);
        // Debug.Log(luaFixedUpdate!=null);
    }
    
    
    private void LoadScript(string scriptName)
    {
        SafeDoString(string.Format("require(\"{0}\")", scriptName));
    }
    public void SafeDoString(string scriptContent, string chunkName="chunk")
    {
        if (luaEnv != null)
        {
            try
            {
                luaEnv.DoString(scriptContent, chunkName);
            }
            catch (Exception e)
            {
                string msg = string.Format("xLua exception : {0}\n {1}", e.Message, e.StackTrace);
                Debug.LogError(msg, null);
            }
        }
    }
    
    public void StartLogin(Action on_ok)
    {
        onLoginOk = on_ok;
        LoadScript("LuaMain");
        SafeDoString("LuaMain()");
    }
    
    //登录成功后，Login面板会调用此方法
    public void OnLoginOk()
    {
        Debug.Log("XLuaManager onLoginOk");
        onLoginOk();
        onLoginOk = null;
    }

    
    public LuaEnv GetLuaEnv()
    {
        return luaEnv;
    }
    

    //**********************************************************************//
    //-----------------------------  LifeTime  
    //**********************************************************************//
    #region LifeTime
    private void Update()
    {
        if (luaEnv != null)
        {
            luaEnv.Tick();
            if (luaUpdate != null)
            {
                try
                {
                    luaUpdate(Time.deltaTime.ToString(), Time.unscaledDeltaTime.ToString());
                }
                catch (Exception ex)
                {
                    Debug.LogError("luaUpdate err : " + ex.Message + "\n" + ex.StackTrace);
                }
            }
        }
    }

    private void LateUpdate() {
        if (luaLateUpdate != null)
        {
            try
            {
                luaLateUpdate();
            }
            catch (Exception ex)
            {
                Debug.LogError("luaLateUpdate err : " + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
    
    private void FixedUpdate() {
        if (luaFixedUpdate != null)
        {
            try
            {
                luaFixedUpdate(Time.fixedDeltaTime.ToString());
            }
            catch (Exception ex)
            {
                Debug.LogError("luaFixedUpdate err : " + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }

    private void OnDestroy()
    {
        if (luaEnv!=null)
        {
            try
            {
                luaEnv.Dispose();
                luaEnv = null;
            }
            catch (Exception e)
            {
                string msg = string.Format("xLua exception : {0}\n {1}", e.Message, e.StackTrace);
                Debug.LogError(msg, null);
            }
        }
    }

    #endregion
    
    public static byte[] CustomLoader(ref string filepath)
    {
        string scriptPath = string.Empty;
        filepath = filepath.Replace(".", "/") + ".lua";


        scriptPath = Path.Combine(AppConfig.LuaAssetsDir, filepath);
        // Debug.Log("Load lua script : " + scriptPath);
        return File.ReadAllBytes(scriptPath);
    }
}

