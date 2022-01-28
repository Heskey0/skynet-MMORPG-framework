using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 用于接受触摸相关的事件
/// </summary>
public class TouchEx : MonoBehaviour, IDragHandler,IPointerDownHandler,IPointerUpHandler
{
    //拖动事件
    public Action<PointerEventData> DragCallBack;
    //pointerDown
    public Action<PointerEventData> PointerDownCallBack;
    public Action<PointerEventData> PointerUpCallBack;
    
    public void OnDrag(PointerEventData eventData)
    {
        if(DragCallBack != null)
        {
            DragCallBack(eventData);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PointerDownCallBack!=null)
        {
            PointerDownCallBack(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (PointerUpCallBack !=null)
        {
            PointerUpCallBack(eventData);
        }
    }
}
