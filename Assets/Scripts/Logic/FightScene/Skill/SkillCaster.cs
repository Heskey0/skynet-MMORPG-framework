using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 负责技能的释放（流程部分）
/// </summary>
public class SkillCaster
{
    Role _owner;

    public bool IsCasting { get { return _castingSkill!= null; } }
    private SkillFIreBallLogic _castingSkill;

    internal void Init(Role owner)
    {
        _owner = owner;
    }

    internal void CastSkill(SkillFIreBallLogic skillLogic,Transform target)
    {
        skillLogic.Start(target,onSkillLogicEnd);
    }

    private void onSkillLogicEnd()
    {
        _castingSkill = null;
    }

    internal void Loop()
    {
        throw new NotImplementedException();
    }
}
