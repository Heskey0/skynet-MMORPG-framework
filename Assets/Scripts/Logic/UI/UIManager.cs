using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI管理器
/// </summary>
public class UIManager : Singleton<UIManager>
{
    //事件系统启用
    public bool EventSystemEnabled
    {
        get
        {
            return _event.enabled;
        }
        set
        {
            _event.enabled = value;
        }
    }
    
    
    //UI管理者
    private GameObject _uiRoot;
    private EventSystem _event;

    //存储所有层的根物体
    //层次 根节点
    Dictionary<UILayer, GameObject> _uiLayerRoot = new Dictionary<UILayer, GameObject>();
    public void Init()
    {
        if (_uiRoot == null)
        {
            _uiRoot = ResMgr.Instance.GetInstance("UI/UISystem");
            GameObject.DontDestroyOnLoad(_uiRoot);

            _event = _uiRoot.Find<EventSystem>("EventSystem");
        }
        
        _uiLayerRoot.Add(UILayer.Scene, _uiRoot.transform.Find("Canvas/Scene").gameObject);
        _uiLayerRoot.Add(UILayer.Touch, _uiRoot.transform.Find("Canvas/Touch").gameObject);
        
        _uiLayerRoot.Add(UILayer.FightUI, _uiRoot.transform.Find("Canvas/FightUI").gameObject);
        _uiLayerRoot.Add(UILayer.Normal, _uiRoot.transform.Find("Canvas/Normal").gameObject);
        _uiLayerRoot.Add(UILayer.Top, _uiRoot.transform.Find("Canvas/Top").gameObject);
        
    }



    public GameObject Add(string uiPath,UILayer layer)
    {
        var root = ResMgr.Instance.GetInstance(uiPath);
        root.transform.SetParent(_uiLayerRoot[layer].transform, false);
        return root;
    }

    public void RemoveLayer(UILayer layer = UILayer.Normal)
    {
        _uiLayerRoot[layer].DestroyAllChildren();
    }
    public void Remove(GameObject ui)
    {
        ResMgr.Instance.Release(ui);
    }

    /// <summary>
    /// 替换ui
    /// </summary>
    /// <param name="uiPath"></param>
    /// <param name="layer">默认值为Normal</param>
    public GameObject Replace(string uiPath, UILayer layer = UILayer.Normal)
    {
        RemoveLayer(layer);
        return Add(uiPath,layer);
    }
}

public enum UILayer
{
    Scene,      //角色血条
    Touch,      //与场景交互
    FightUI,    //小地图 角色头像
    Normal,     //背包
    Top         //世界公告
}