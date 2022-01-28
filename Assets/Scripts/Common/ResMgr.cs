using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Դ������
/// ���ط�ʽ �� ʹ���߼�����
/// �Ա���֧���ȸ��£������
/// </summary>
public class ResMgr : Singleton<ResMgr>
{
    public void Init()
    {
        ObjectPoolMgr.Instance.CreatePool("aa",1,3, GetResources<GameObject>("path"));
    }

    public GameObject GetInstance(string resPath)
    {
        //�ȴӶ��������
        if (ObjectPoolMgr.Instance.GetPool(resPath) != null)
        {
            return ObjectPoolMgr.Instance.Get(resPath);
        }

        //û�ж�Ӧ�Ķ����
        return GameObject.Instantiate(GetResources<GameObject>(resPath));
    }

    public T GetResources<T>(string resPath) where T : Object
    {
        return Resources.Load<T>(resPath);
    }

    public void Release(GameObject ui)
    {
        var creature = ui.GetComponent<Creature>();
        if (creature!=null)
        {
            ObjectPoolMgr.Instance.Release();
        }
        GameObject.Destroy(ui);
    }
}
