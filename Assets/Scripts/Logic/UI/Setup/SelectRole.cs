using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectRole : MonoBehaviour
{
    private Button _btnEnter;
    private GameObject _roleListContent;
    private ToggleGroup _roleListToggleGroup;

    private TouchEx _touchEx; //模型旋转
    //private GameObject _modelStudio;//模型摄影棚
    //private GameObject _modelPlace;//模型放置节点

    private ModelStudio _modelStudio;

    private int _selectedRoleIndex = -1;

    private void Awake()
    {
        _btnEnter = transform.Find("BtnEnter").GetComponent<Button>();
        _roleListContent = transform.Find("RoleList/Viewport/Content").gameObject;
        _roleListToggleGroup = _roleListContent.GetComponentInParent<ToggleGroup>();

        _btnEnter.onClick.AddListener(OnBtnEnterClicked);

        //模型摄影棚
        _modelStudio = new ModelStudio();
        _modelStudio.Init();
        _touchEx = gameObject.Find<TouchEx>("ImgTouchRotate");
        _touchEx.DragCallBack = onTouchRotate;

        //初始化角色列表
        int i = 0;
        //foreach (var roleInfo in UserData.Instance.AllRole)
        foreach (var roleInfo in RoleTable.Instance.GetAll())
        {
            var roleItem = ResMgr.Instance.GetInstance("UI/SelectRole/RoleItem");
            roleItem.transform.SetParent(_roleListContent.transform, false); //是否保持世界坐标不变
            var txtName = roleItem.transform.Find("Label").GetComponent<Text>();
            var toggle = roleItem.GetComponent<Toggle>();
            txtName.text = roleInfo.Value.Name;

            //(闭包)绑定 角色索引 和 toggle
            var index = i;
            i++;
            toggle.isOn = false;
            toggle.onValueChanged.AddListener((isOn) => { onToggleValueChanged(index); });
            if (i == 1) toggle.isOn = true; //默认选中第一个
        }
    }

    /// <summary>
    /// 拖动事件
    /// </summary>
    /// <param name="obj"></param>
    private void onTouchRotate(PointerEventData eventData)
    {
        _modelStudio.ModelPlace.transform.Rotate(new Vector3(0, -eventData.delta.x, 0));
    }

    /// <summary>
    /// 选中toggle时调用
    /// </summary>
    /// <param name="roleIndex"></param>
    private void onToggleValueChanged(int roleIndex)
    {
        //如果点击 另一个toggle
        if (_selectedRoleIndex != roleIndex)
        {
            //清空ModelPlace下的模型
            _modelStudio.ClearModel();
            //生成新的模型
            var curRoleInfo = UserData.Instance.AllRole[roleIndex];
            var modelPath = RoleTable.Instance[curRoleInfo.ModelId].ModelPath;
            var model = ResMgr.Instance.GetInstance(modelPath);

            _modelStudio.SetModel(model);
            model.transform.localPosition = Vector3.zero;
            Debug.Log(curRoleInfo.Name);
        }

        _selectedRoleIndex = roleIndex;
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    private void OnBtnEnterClicked()
    {
        //发送一个选中角色消息
        SelectRoleCmd cmd = new SelectRoleCmd() {Index = _selectedRoleIndex};
        Net.Instance.SendCmd(cmd);

        UIManager.Instance.RemoveLayer();
    }
}