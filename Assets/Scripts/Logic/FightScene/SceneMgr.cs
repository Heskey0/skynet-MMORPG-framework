using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景管理
/// </summary>
public class SceneMgr : Singleton<SceneMgr>
{
    public static CameraController MainCameraController;
    internal static void OnEnterMap(Cmd cmd)
    {
        if (!Net.CheckCmd(cmd, typeof(EnterMapCmd)))
        {
            return;
        }
        EnterMapCmd enterMapCmd = cmd as EnterMapCmd;
        

        reset();

        SceneMgr.Instance.loadScene(enterMapCmd.MapID, () =>
        {
            InitCamera();
            FightUIMgr.Instance.Init();
        });
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="mapID"></param>
    public void loadScene(int mapID,Action loadcallBack=null)
    {
        var mapDataBase = MapTable.Instance[mapID];
        if (mapDataBase ==null)
        {
            Debug.LogError("未找到地图" + mapID);
            return;
        }

        UIManager.Instance.EventSystemEnabled = false;
        //loading界面
        //阻塞消息
        Net.Instance.Pause = true;
        var ao = SceneManager.LoadSceneAsync(mapDataBase.ScenePath);
        QuickCoroutine.Instance.StartCoroutine(LoadEnd(ao,loadcallBack));



        //Debug.Log( LoadEnd(ao));
        //SceneManager.LoadScene(mapDataBase.ScenePath);
    }


    private IEnumerator LoadEnd(AsyncOperation ao,Action loadCallBack)
    {
        while ( !ao.isDone)
        {
            //加载进度 ao.progress
            //Debug.Log("正在加载");
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("场景加载完毕");
        UIManager.Instance.EventSystemEnabled = true;
        loadCallBack?.Invoke();

        
        //分发阻塞的消息
        Net.Instance.Pause = false;
        //销毁loading
    }

    /// <summary>
    /// 重置场景
    /// </summary>
    private static void reset()
    {
        UIManager.Instance.RemoveLayer();
        RoleMgr.Instance.Reset();
        NpcMgr.Instance.Reset();
        FightUIMgr.Instance.Reset();
    }
    

    private static void InitCamera()
    {
        Debug.Log("initCamera");
        var cameraObj = ResMgr.Instance.GetInstance("SceneCamera");
        MainCameraController = cameraObj.GetComponent<CameraController>();
    }
}
