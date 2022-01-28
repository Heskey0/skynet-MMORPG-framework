using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能对象
/// </summary>
public class SkillObject
{
    public SkillDatabase TableData;//技能静态数据
    public SkillLogicBase Logic;//技能动态数据（逻辑）
}

/// <summary>
/// 技能数据（静态部分）
/// </summary>
public class SkillMgr
{
    Role _owner;

    List<SkillObject> _allSkill = new List<SkillObject>();
    SkillFIreBallLogic _skillLogic;
    SkillCaster _skillCaster = new SkillCaster();
    public bool IsCasting { get { return _skillCaster.IsCasting; } }
    public void Init(Role owner)
    {
        _skillCaster.Init(_owner);
        for (int i = 0; i < GameSettings.MaxSkillNum; i++)
        {
            var skillObject = new SkillObject();

            //i为角色表中角色拥有的技能
            //var skillid = skillidList[i];
            var skillid = i;
            skillObject.TableData = SkillTable.Instance[skillid];
            skillObject.Logic = new SkillLogicBase();


            _allSkill.Add(skillObject);
        }
        _skillLogic = new SkillFIreBallLogic();
        _owner = owner;
    }

    public void TryCastSkill(int index)
    {
        if (_skillCaster.IsCasting) { return; }
        //找到技能

        //CD
        //技能目标

        //开始施放技能
        CastSkill();
    }

    private void CastSkill()
    {
        _owner.StopMove();
        _skillCaster.CastSkill(_skillLogic, null);
    }

    private void StartSkill(int index)
    {
        _skillLogic = new SkillFIreBallLogic();
        _skillLogic.Init(_owner);
        _skillLogic.Start(null,null);//设置攻击目标
    }

    public void Loop()
    {
        _skillCaster.Loop();
    }

}
