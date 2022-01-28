using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����
/// </summary>
public class MainRole : Role
{
   

    public override void Init(CreateSceneRoleCmd serverData, RoleDataBase tableData)
    {
        base.Init(serverData, tableData);
        gameObject.layer = LayerMask.NameToLayer("MainRole");
        bindingControlEvent();
    }

    /// <summary>
    /// ����������ʱ����
    /// </summary>
    private void bindingControlEvent()
    {
        // �󶨿����¼���UI����Ҫ �������ǣ�
        FightUIMgr.Instance.BindingJoyStick(OnJoystickMove, OnJoystickMoveEnd);
        FightUIMgr.Instance.BindingTouchScene(onTouchSomething,gameObject.layer);
        
        FightUIMgr.Instance.BindingSkillBtn(onSkill);
    }
    
    /// <summary>
    /// ҡ���ƶ�
    /// </summary>
    /// <param name="dir"></param>
    private void OnJoystickMove(Vector2 dir)
    {
        var destination = transform.position + new Vector3(dir.x, 0, dir.y)*_agent.speed*0.2f;
        JoystickMove(destination);
    }

    private void OnJoystickMoveEnd()
    {
        StopMove();
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="hit"></param>
    private void onTouchSomething(RaycastHit hit)
    {
        var hitObj = hit.transform.gameObject;
        if (hitObj.layer == LayerMask.NameToLayer("Ground"))
        {
            TouchGroundPathTo(hit.point);
            return;
        }

        var npc = hitObj.GetComponent<Npc>();
        if (npc!=null)
        {
            onVisitNpc(npc);
        }
    }

    private void onVisitNpc(Npc npc)
    {
        //�ƶ�����
        PurposeTo(npc.transform.position,1, () =>
        {
            //onVisitNpc(npc);
            Debug.Log("visit :" + npc.Name);

        });
    }
    
    public void OnJumpTo(int JumpToMapID)
    {
        Net.Instance.SendCmd(new JumpTo(){ID = JumpToMapID});
    }

    /// <summary>
    /// ���� ����ʩ��
    /// </summary>
    /// <param name="index"></param>
    private void onSkill(int index)
    {
    }


}