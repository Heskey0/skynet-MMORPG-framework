//code by 赫斯基皇
//https://space.bilibili.com/455965619
//https://github.com/Heskey0

using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 点击场景的消息
/// </summary>
public class TouchScene
{
    public Action<RaycastHit> HitSthCallback;
    public int UnTouchLayer;    //不被射线检测的层
    
    private GameObject _root;
    
    public TouchScene()
    {
        _root = UIManager.Instance.Add("UI/FightUI/TouchScene", UILayer.Touch);

        var touchEx = _root.GetComponent<TouchEx>();
        touchEx.PointerUpCallBack = onTouchScene;
    }

    private void onTouchScene(PointerEventData eventData)
    {
        var ray = CameraController.Instance.GetComponent<Camera>().ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,float.MaxValue,~(1<<UnTouchLayer)))
        {
            if (HitSthCallback!=null)
            {
                HitSthCallback(hit);
            }
        }
    }
}
