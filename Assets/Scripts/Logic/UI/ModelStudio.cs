using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ģ����Ӱ��
/// </summary>
public class ModelStudio
{
    protected TouchEx _touchEx;//ģ����ת
    protected GameObject _modelStudio;//ģ����Ӱ��
    public GameObject Root => _modelStudio;

    protected GameObject _modelPlace;//ģ�ͷ��ýڵ�
    public GameObject ModelPlace => _modelPlace;

    
    protected GameObject _modelRoot;//ģ��

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
