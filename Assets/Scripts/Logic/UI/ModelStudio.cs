using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 模型摄影棚
/// </summary>
public class ModelStudio
{
    protected TouchEx _touchEx;//模型旋转
    protected GameObject _modelStudio;//模型摄影棚
    public GameObject Root => _modelStudio;

    protected GameObject _modelPlace;//模型放置节点
    public GameObject ModelPlace => _modelPlace;

    
    protected GameObject _modelRoot;//模型

    public virtual void Init()
    {
        _modelStudio = ResMgr.Instance.GetInstance("UI/SelectRole/ModelStudio");
        _modelPlace = _modelStudio.Find<Transform>("ModelPlace").gameObject;
        //touchRotate.DragCallBack = onTouchRotate;
    }


    public void ClearModel()
    {
        _modelPlace.DestroyAllChildren();
    }

    public void SetModel(GameObject modelRoot)
    {
        _modelRoot = modelRoot;
        modelRoot.transform.SetParent(_modelPlace.transform,false);
        modelRoot.transform.localPosition = Vector3.zero;
    }
    public void Destory()
    {
        ResMgr.Instance.Release(_modelStudio);
    }
}
