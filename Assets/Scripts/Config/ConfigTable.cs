using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

/// <summary>
/// 表格的每行数据
/// </summary>
public class TableDatabase
{
    public int ID;

}


/// <summary>
/// 表格基类
/// </summary>
public class ConfigTable<TDataBase, T> : Singleton<T>
    where TDataBase : TableDatabase, new()
    where T : class, new()
{
    //相同
    //id 数据条目
    //索引方式
    //数据存储方式
    //表的读取方式
    Dictionary<int, TDataBase> _cache = new Dictionary<int, TDataBase>();

    protected void load(string tablePath)
    {

        MemoryStream tableStream;
        // 开发期，读Project/Config
#if UNITY_EDITOR
        var srcPath = Application.dataPath + "/../" + tablePath;
        tableStream = new MemoryStream(File.ReadAllBytes(srcPath));
#else
        // 发布期，读Resources/Config
        //二进制方式读表
        var table = Resources.Load<TextAsset>(tablePath);
        tableStream = new MemoryStream(table.bytes);
#endif


        //table的二进制作为内存流的数据源

        using (var reader = new StreamReader(tableStream, Encoding.GetEncoding("gb2312")))
        {
            //跳过注释（表中第一行）
            reader.ReadLine();

            //字段名 使用反射
            var fileNameStr = reader.ReadLine();
            var fileNameArray = fileNameStr.Split(',');
            List<FieldInfo> allFileInfo = new List<FieldInfo>();
            foreach (var fieldName in fileNameArray)
            {
                var fieldType = typeof(TDataBase).GetField(fieldName);
                if (fieldType!=null)
                {
                    //字符串映射到字段 并将字段加入列表
                    allFileInfo.Add(fieldType);
                }
                else
                {
                    Debug.LogError("表中字段"+""+"未在程序中定义"+ fieldName);
                }
            }


            //从第二行开始读
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
        //从第二行开始读到的数据
        var itemStrArray = lineStr.Split(',');//分割
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
    /// 索引器 获取表格数据_cache
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

    //不同
    //数据类型不同
    //文件路径不同
}
