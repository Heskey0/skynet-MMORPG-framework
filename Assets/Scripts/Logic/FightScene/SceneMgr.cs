using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ��������
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
    /// ���س���
    /// </summary>
    /// <param name="mapID"></param>
    public void loadScene(int mapID,Action loadcallBack=null)
    {
        var mapDataBase = MapTable.Instance[mapID];
        if (mapDataBase ==null)
        {
            Debug.LogError("δ�ҵ���ͼ" + mapID);
            return;
        }

        UIManager.Instance.EventSystemEnabled = false;
        //loading����
        //������Ϣ
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
            //���ؽ��� ao.progress
            //Debug.Log("���ڼ���");
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("�����������");
        UIManager.Instance.EventSystemEnabled = true;
        loadCallBack?.Invoke();

        
        //�ַ���������Ϣ
        Net.Instance.Pause = false;
        //����loading
    }

    /// <summary>
    /// ���ó���
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
