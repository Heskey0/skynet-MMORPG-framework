using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������˵Ľ�ɫģ��
/// </summary>
public class RoleServer
{
    static int _curThisID = 1;
    //���ɽ�ɫ��thisID
    public static int GetNewThisID()
    {
        return ++_curThisID;
    }

    public int ThisID;
    public string Name;//��ɫ��
    public int ModelId;//ģ��ID
    //��������Ѫ
    //��ɫ����
    //���ڵ�ͼ
    //������Ϣ
}
