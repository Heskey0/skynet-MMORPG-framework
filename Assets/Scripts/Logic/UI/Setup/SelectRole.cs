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

    private TouchEx _touchEx; //ģ����ת
    //private GameObject _modelStudio;//ģ����Ӱ��
    //private GameObject _modelPlace;//ģ�ͷ��ýڵ�

    private ModelStudio _modelStudio;

    private int _selectedRoleIndex = -1;

    private void Awake()
    {
        _btnEnter = transform.Find("BtnEnter").GetComponent<Button>();
        _roleListContent = transform.Find("RoleList/Viewport/Content").gameObject;
        _roleListToggleGroup = _roleListContent.GetComponentInParent<ToggleGroup>();

        _btnEnter.onClick.AddListener(OnBtnEnterClicked);

        //ģ����Ӱ��
        _modelStudio = new ModelStudio();
        _modelStudio.Init();
        _touchEx = gameObject.Find<TouchEx>("ImgTouchRotate");
        _touchEx.DragCallBack = onTouchRotate;

        //��ʼ����ɫ�б�
        int i = 0;
        //foreach (var roleInfo in UserData.Instance.AllRole)
        foreach (var roleInfo in RoleTable.Instance.GetAll())
        {
            var roleItem = ResMgr.Instance.GetInstance("UI/SelectRole/RoleItem");
            roleItem.transform.SetParent(_roleListContent.transform, false); //�Ƿ񱣳��������겻��
            var txtName = roleItem.transform.Find("Label").GetComponent<Text>();
            var toggle = roleItem.GetComponent<Toggle>();
            txtName.text = roleInfo.Value.Name;

            //(�հ�)�� ��ɫ���� �� toggle
            var index = i;
            i++;
            toggle.isOn = false;
            toggle.onValueChanged.AddListener((isOn) => { onToggleValueChanged(index); });
            if (i == 1) toggle.isOn = true; //Ĭ��ѡ�е�һ��
        }
    }

    /// <summary>
    /// �϶��¼�
    /// </summary>
    /// <param name="obj"></param>
    private void onTouchRotate(PointerEventData eventData)
    {
        _modelStudio.ModelPlace.transform.Rotate(new Vector3(0, -eventData.delta.x, 0));
    }

    /// <summary>
    /// ѡ��toggleʱ����
    /// </summary>
    /// <param name="roleIndex"></param>
    private void onToggleValueChanged(int roleIndex)
    {
        //������ ��һ��toggle
        if (_selectedRoleIndex != roleIndex)
        {
            //���ModelPlace�µ�ģ��
            _modelStudio.ClearModel();
            //�����µ�ģ��
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
    /// ������Ϸ
    /// </summary>
    private void OnBtnEnterClicked()
    {
        //����һ��ѡ�н�ɫ��Ϣ
        SelectRoleCmd cmd = new SelectRoleCmd() {Index = _selectedRoleIndex};
        Net.Instance.SendCmd(cmd);

        UIManager.Instance.RemoveLayer();
    }
}