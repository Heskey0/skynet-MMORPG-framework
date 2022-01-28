using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    static GameSettings()
    {
        MainRoleLayer = LayerMask.NameToLayer("MainRole");
    }

    public static int MainRoleLayer;
    // �ƶ�ֹͣ���������
    public const float StopDistance = 0.1f;
    
    public const int MaxSkillNum = 5;
}
