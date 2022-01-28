using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 管理所有对话框
/// </summary>
public class DialogMgr : Singleton<DialogMgr>
{
    List<Dialog> _allDlg = new List<Dialog>();


    public T Open<T>() where T : Dialog, new()
    {
        var dlg = new T();
        _allDlg.Add(dlg);

        return dlg;
    }

    public void Close(Dialog dlg)
    {
        dlg.CloseSelf();
        _allDlg.Remove(dlg);
    }

    public void CloseAll()
    {
        foreach (var dlg in _allDlg)
        {
            dlg.CloseSelf();
        }

        _allDlg.Clear();
    }
}


/// <summary>
/// 所有对话框的基类
/// </summary>
public abstract class Dialog
{
    protected GameObject _root;

    public bool IsAlive
    {
        get { return _root != null; }
    }

    protected void load(string uiPath)
    {
        _root = UIManager.Instance.Add(uiPath, UILayer.Normal);

        var btnClose = _root.Find<Button>("BtnClose");
        if (btnClose != null)
        {
            btnClose.onClick.AddListener(CloseSelf);
        }
    }

    public virtual void CloseSelf()
    {
        UIManager.Instance.Remove(_root);
        onClose();
    }

    protected virtual void onClose()
    {
    }
}