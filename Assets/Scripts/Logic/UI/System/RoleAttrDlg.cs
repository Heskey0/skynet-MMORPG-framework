using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleAttrDlg:Dialog
{
    //private UIModelStudio _modelStudio;
    private UIModel _uiModel;
    
    public RoleAttrDlg()
    {
        load("UI/System/RoleAttr/RoleAttrDlg");

        var modelArea = _root.Find<RectTransform>("RoleModel");
        
        //����ģ��
        var modelImage = _root.Find<RawImage>("RoleModel");
        var mainRole = RoleMgr.Instance.MainRole;
        if (mainRole == null) { Debug.LogError("δ�ҵ�����");return; }
        _uiModel=UIModelMgr.Instance.CreateUIModel((int)modelArea.rect.width
            , (int)modelArea.rect.height
            , modelImage
            , ResMgr.Instance.GetInstance(mainRole.ModelPath));


        
    }

    protected override void onClose()
    {
        //_modelStudio.Destory();
        //_uiModel.Destroy();
        UIModelMgr.Instance.Release(_uiModel);
    }
}

