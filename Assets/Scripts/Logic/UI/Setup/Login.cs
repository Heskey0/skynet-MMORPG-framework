using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SprotoType;
using Sproto;



/// <summary>
/// 登录界面
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
        //账号密码格式检验
        var account = _inputAccount.text;
        var password = _inputPassword.text;

        loginCallback?.Invoke();
        if (AppConfig.DebugMode)
        {

            // 调试模式：使用虚拟服务器
            Net.Instance.ConnectServer(doSuccess, doFailed);
        }
        else
        {
            // 真机模式
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
    /// 连接成功
    /// </summary>
    private void doSuccess(byte[] bytes)
    {

        var account = _inputAccount.text;
        var password = _inputPassword.text;

        if (AppConfig.DebugMode)
        {
            XLuaManager.Instance.OnLoginOk();
            Debug.Log("连接到本地虚拟服务器");
            var cmd = new LoginCmd() {Account = account, Password = password};
            Net.Instance.SendCmd(cmd);
        }
        else
        {
            Debug.Log("连接到服务器：" + AppConfig.ServerIp + ":" + AppConfig.Port);

            var req = new SprotoType.account.request();
            req.account = 0;
            req.password = "";
            NetMsgDispatcher.Instance.SendMessage<Protocol.account>(req, null);
        }
        

        // UserData.Instance.AllRole = roleListCmd.AllRole;
        //
        // if (roleListCmd.AllRole.Count>0)
        // {
        //     //选人界面
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
    /// 连接失败
    /// </summary>
    private void doFailed(byte[] bytes)
    {
        Debug.LogError("连接服务器失败");

        _inputAccount.interactable = true;
        _inputPassword.interactable = true;
        _btnOK.interactable = true;
        NetworkManager.Instance.OnDisConnectCallBack -= doFailed;
    }
}