//code by 赫斯基皇
//https://space.bilibili.com/455965619
//https://github.com/Heskey0

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModel
{
    private UIModelStudio _modelStudio;

    public UIModel(int width,int height,RawImage targetImage,GameObject modelRoot)
    {
        //加载模型
        _modelStudio = new UIModelStudio();
        _modelStudio.Init();
        _modelStudio.SetModel(modelRoot);

        var camera = _modelStudio.Root.Find<Camera>("Camera");
        var texture = new RenderTexture(width,height,24);
        camera.targetTexture = texture;
        
        //画texture
        targetImage.texture = texture;
    }

    public void SetStudioPosition(ref Vector3 pos)
    {
        _modelStudio.Root.transform.position = pos;
    }
    
    public void Destroy()
    {
        _modelStudio.Destory();
    }
}

/// <summary>
/// 管理所有的UIModel，防止UIModel重叠
/// </summary>
public class UIModelMgr : Singleton<UIModelMgr>
{
    List<UIModel> _allUIModel = new List<UIModel>();
    
    private readonly Vector3 _srcPos=new Vector3(0,100,0);

    private readonly float _deltaY = 20;

    private Vector3 _curPos;

    public UIModelMgr()
    {
        _curPos = _srcPos;
    }
    
    public UIModel CreateUIModel(int width,int height,RawImage targetImage,GameObject modelRoot)
    {
        var uiModel = new UIModel(width, height, targetImage, modelRoot);
        uiModel.SetStudioPosition(ref _curPos);
        _curPos = _curPos + new Vector3(0,_deltaY,0);
        
        _allUIModel.Add(uiModel);
        return uiModel;
    }

    public void Release(UIModel uiModel)
    {
        uiModel.Destroy();
        _allUIModel.Remove(uiModel);
    }
}
