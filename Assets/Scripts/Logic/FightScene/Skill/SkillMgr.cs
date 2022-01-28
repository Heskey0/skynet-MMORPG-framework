using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ܶ���
/// </summary>
public class SkillObject
{
    public SkillDatabase TableData;//���ܾ�̬����
    public SkillLogicBase Logic;//���ܶ�̬���ݣ��߼���
}

/// <summary>
/// �������ݣ���̬���֣�
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

            //iΪ��ɫ���н�ɫӵ�еļ���
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
        //�ҵ�����

        //CD
        //����Ŀ��

        //��ʼʩ�ż���
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
        _skillLogic.Start(null,null);//���ù���Ŀ��
    }

    public void Loop()
    {
        _skillCaster.Loop();
    }

}
