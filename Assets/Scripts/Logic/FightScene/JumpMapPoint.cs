//code by 赫斯基皇
//https://space.bilibili.com/455965619
//https://github.com/Heskey0

using System;
using UnityEngine;

public class JumpMapPoint : MonoBehaviour
{
    public int JumpToMapID = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer!=GameSettings.MainRoleLayer)
        {
            return;
        }
        
        //Debug.Log("跳转场景"+JumpToMapID);
        other.GetComponent<MainRole>().OnJumpTo(JumpToMapID);

    }
}
