//code by 赫斯基皇
//https://space.bilibili.com/455965619
//https://github.com/Heskey0

using System;
using UnityEngine;

public class UIModelStudio:ModelStudio
{
    public override void Init()
    {
        _modelStudio = ResMgr.Instance.GetInstance("UI/System/UIModelStudio");
        _modelPlace = _modelStudio.Find<Transform>("ModelPlace").gameObject;
    }
}
