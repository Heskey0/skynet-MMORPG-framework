using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 技能操作面板
/// </summary>
public class SkillAtkDlg
{
    GameObject _root;
    List<SkillPanel> allSkillPanel = new List<SkillPanel>();
    public Action<int> OnSkillBtnClick;

    public SkillAtkDlg()
    {
        _root = UIManager.Instance.Add("",UILayer.FightUI);
        for (int i = 0; i < GameSettings.MaxSkillNum; i++)
        {
            var skillPanel = new SkillPanel();
            var index = i + 1;
            string panelName = "SkillPanel" + index.ToString();
            skillPanel.Btn = _root.Find<Button>(panelName);
            skillPanel.CountDown = _root.Find<Image>(panelName+"/CountDownImage");
            skillPanel.CDText = _root.Find<Text>(panelName+"/CountDownLabel");

            //闭包
            skillPanel.Btn.onClick.AddListener(()=>onSkillClick(index));
            allSkillPanel.Add(skillPanel);
        }
    }

    private void onSkillClick(int index)
    {
        if (OnSkillBtnClick != null) { OnSkillBtnClick(index); }
    }
}

/// <summary>
/// 技能面板中 单个技能相关组件打包
/// </summary>
public class SkillPanel
{
    public Button Btn;
    public Image CountDown;
    public Text CDText;
}