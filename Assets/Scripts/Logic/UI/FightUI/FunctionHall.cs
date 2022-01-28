using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ¹¦ÄÜ´óÌü
/// </summary>
public class FunctionHall
{
    GameObject _root;
    public FunctionHall()
    {
        _root = UIManager.Instance.Add("UI/FightUI/FunctionHall", UILayer.FightUI);

        var btnRoleAttr = _root.Find<Button>("BtnRole");
        btnRoleAttr.onClick.AddListener(onRoleAttrClick);
    }

    private void onRoleAttrClick()
    {
        //UIManager.Instance.Add("UI/System/RoleAttr/RoleAttrDlg",UILayer.Normal);
        DialogMgr.Instance.Open<RoleAttrDlg>();
    }
}
