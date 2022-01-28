using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SprotoType;
using Sproto;



/// <summary>
/// ��¼����
/// </summary>
public class Login : MonoBehaviour
{
    private InputField _inputAccount;
    private InputField _inputPassword;
    private Button _btnOK;

    public event Action loginCallback;

    private void Awake()
    {
        _inputAccount = gameObject.Find<InputField>("InputAccount");
        _inputPassword = gameObject.Find<InputField>("InputPassword");
        _btnOK = gameObject.Find<Button>("BtnLogin");
        _btnOK.onClick.AddListener(onBtnOKClicked);
    }

    private void onBtnOKClicked()
    {
        //�˺������ʽ����
        var account = _inputAccount.text;
        var password = _inputPassword.text;

        loginCallback?.Invoke();
        if (AppConfig.DebugMode)
        {

            // ����ģʽ��ʹ�����������
            Net.Instance.ConnectServer(doSuccess, doFailed);
        }
        else
        {
            // ���ģʽ
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                //return;
            }

            try
            {
                NetworkManager.Instance.OnConnectCallBack += doSuccess;
                NetworkManager.Instance.OnDisConnectCallBack += doFailed;
                NetworkManager.Instance.SendConnect(AppConfig.ServerIp, AppConfig.Port, NetPackageType.BaseHead);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                
            }
        }

        _inputAccount.interactable = false;
        _inputPassword.interactable = false;
        _btnOK.interactable = false;
    }


    /// <summary>
    /// ���ӳɹ�
    /// </summary>
    private void doSuccess(byte[] bytes)
    {

        var account = _inputAccount.text;
        var password = _inputPassword.text;

        if (AppConfig.DebugMode)
        {
            XLuaManager.Instance.OnLoginOk();
            Debug.Log("���ӵ��������������");
            var cmd = new LoginCmd() {Account = account, Password = password};
            Net.Instance.SendCmd(cmd);
        }
        else
        {
            Debug.Log("���ӵ���������" + AppConfig.ServerIp + ":" + AppConfig.Port);

            var req = new SprotoType.account.request();
            req.account = 0;
            req.password = "";
            NetMsgDispatcher.Instance.SendMessage<Protocol.account>(req, null);
        }
        

        // UserData.Instance.AllRole = roleListCmd.AllRole;
        //
        // if (roleListCmd.AllRole.Count>0)
        // {
        //     //ѡ�˽���
        //     SceneManager.LoadScene("2.SelectRole");
        //     SceneMgr.Instance.loadScene(2, () =>
        //     {
        //         UIManager.Instance.Replace("UI/SelectRole/SelectRole");
        //     });
        //     //UIManager.Instance.RemoveLayer();
        //     //UIManager.Instance._uiRoot.SetActive(false);
        //
        //
        // }
        // else
        // {
        //     SceneManager.LoadScene("CreateRole");
        // }

        NetworkManager.Instance.OnConnectCallBack -= doSuccess;
    }

    /// <summary>
    /// ����ʧ��
    /// </summary>
    private void doFailed(byte[] bytes)
    {
        Debug.LogError("���ӷ�����ʧ��");

        _inputAccount.interactable = true;
        _inputPassword.interactable = true;
        _btnOK.interactable = true;
        NetworkManager.Instance.OnDisConnectCallBack -= doFailed;
    }
}