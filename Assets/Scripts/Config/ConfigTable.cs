using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

/// <summary>
/// ����ÿ������
/// </summary>
public class TableDatabase
{
    public int ID;

}


/// <summary>
/// ������
/// </summary>
public class ConfigTable<TDataBase, T> : Singleton<T>
    where TDataBase : TableDatabase, new()
    where T : class, new()
{
    //��ͬ
    //id ������Ŀ
    //������ʽ
    //���ݴ洢��ʽ
    //��Ķ�ȡ��ʽ
    Dictionary<int, TDataBase> _cache = new Dictionary<int, TDataBase>();

    protected void load(string tablePath)
    {

        MemoryStream tableStream;
        // �����ڣ���Project/Config
#if UNITY_EDITOR
        var srcPath = Application.dataPath + "/../" + tablePath;
        tableStream = new MemoryStream(File.ReadAllBytes(srcPath));
#else
        // �����ڣ���Resources/Config
        //�����Ʒ�ʽ����
        var table = Resources.Load<TextAsset>(tablePath);
        tableStream = new MemoryStream(table.bytes);
#endif


        //table�Ķ�������Ϊ�ڴ���������Դ

        using (var reader = new StreamReader(tableStream, Encoding.GetEncoding("gb2312")))
        {
            //����ע�ͣ����е�һ�У�
            reader.ReadLine();

            //�ֶ��� ʹ�÷���
            var fileNameStr = reader.ReadLine();
            var fileNameArray = fileNameStr.Split(',');
            List<FieldInfo> allFileInfo = new List<FieldInfo>();
            foreach (var fieldName in fileNameArray)
            {
                var fieldType = typeof(TDataBase).GetField(fieldName);
                if (fieldType!=null)
                {
                    //�ַ���ӳ�䵽�ֶ� �����ֶμ����б�
                    allFileInfo.Add(fieldType);
                }
                else
                {
                    Debug.LogError("�����ֶ�"+""+"δ�ڳ����ж���"+ fieldName);
                }
            }


            //�ӵڶ��п�ʼ��
            var lineStr = reader.ReadLine();
            while (lineStr != null)
            {
                TDataBase DB = readLine(allFileInfo, lineStr);

                _cache[DB.ID] = DB;

                lineStr = reader.ReadLine();
            }
        }

    }

    private static TDataBase readLine(List<FieldInfo> allFileInfo, string lineStr)
    {
        //�ӵڶ��п�ʼ����������
        var itemStrArray = lineStr.Split(',');//�ָ�
        var DB = new TDataBase();

        for (int i = 0; i < allFileInfo.Count; i++)
        {
            var field = allFileInfo[i];
            var data = itemStrArray[i];

            if (field.FieldType == typeof(string))
            {
                field.SetValue(DB, data);
            }
            else if (field.FieldType == typeof(int))
            {
                field.SetValue(DB, int.Parse(data));
            }
            else if (field.FieldType == typeof(float))
            {
                field.SetValue(DB, float.Parse(data));
            }
            else if (field.FieldType == typeof(List<int>))
            { 
                var list = new List<int>();
                foreach (var item in data.Split('$'))
                {
                    list.Add(int.Parse(item));
                }
                field.SetValue(DB, list);
            }
        }

        return DB;
    }




    /// <summary>
    /// ������ ��ȡ�������_cache
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TDataBase this[int id]
    {
        get
        {
            TDataBase db;
            _cache.TryGetValue(id, out db);
            return db;
        }
    }


    public Dictionary<int, TDataBase> GetAll()
    {
        return _cache;
    }

    //��ͬ
    //�������Ͳ�ͬ
    //�ļ�·����ͬ
}
