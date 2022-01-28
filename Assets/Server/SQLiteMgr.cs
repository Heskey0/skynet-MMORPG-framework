using System.Collections;
using System.Collections.Generic;
using System.Data;
using LitJson;
using UnityEngine;
using Mono.Data;
using Mono.Data.Sqlite;

public class SQLiteMgr : Singleton<SQLiteMgr>,IDataBase
{
    private SqliteConnection conn;
    private SqliteCommand cmd;

    private const string PlayerTableName = "PlayerData";
    
    public void Init()
    {
        conn = new SqliteConnection("Data Source=./ServerDB.db");
        conn.Open();
        cmd = conn.CreateCommand();

        _initTables();
    }

    /// <summary>
    /// 使用NoSQL 将数据库中Json解析为Player
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Player GetUserData(int id)
    {
        var dataTable = _executeSelectTable(string.Format(QueryDefine.SELECT_PLAYER_DATA, PlayerTableName, id));
        if (dataTable.Rows.Count<=0)
        {
            return null;
        }

        string json = dataTable.Rows[0][0].ToString();
        return JsonMapper.ToObject<Player>(json);
    }

    public void SavePlayerData(Player playerData)
    {
        _executeNoQuery(string.Format(QueryDefine.INSERT_WITH_UPDATE, PlayerTableName, playerData.ThisID, JsonMapper.ToJson(playerData)));
    }
    
    public void Close()
    {
        conn.Close();
    }

    private void _initTables()
    {
        _executeNoQuery(string.Format(QueryDefine.CREATE_TABLE_BEGIN, PlayerTableName) + QueryDefine.CREATE_TABLE_END);
    }

    private DataTable _executeSelectTable(string sql, SqliteParameter[] parameters = null)
    {
        cmd.CommandText = sql;
        if (parameters!=null)
        {
            cmd.Parameters.AddRange(parameters);
        }
        SqliteDataAdapter adapter = new SqliteDataAdapter(cmd);
        DataTable data = new DataTable();
        adapter.Fill(data);

        return data;
    }

    /// <summary>
    /// 执行一个查询语句，返回一个关联的SQLiteDataReader实例
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    private SqliteDataReader _executeReader(string sql, SqliteParameter[] parameters)
    {
        cmd.CommandText = sql;
        if (parameters!=null)
        {
            cmd.Parameters.AddRange(parameters);
        }
        conn.Open();
        return cmd.ExecuteReader(CommandBehavior.Default);
    }

    /// <summary>
    /// 执行非查询命令
    /// </summary>
    private void _executeNoQuery(string cmdStr,SqliteParameter[] parameters = null)
    {
        cmd.CommandText = cmdStr;

        Debug.Log("<color=red>[write db]</color>" + cmdStr);
        
        if (parameters!=null)
        {
            cmd.Parameters.AddRange(parameters);
        }

        cmd.ExecuteNonQuery();

        cmd.Parameters.Clear();
    }
    
    
}

/// <summary>
/// 通用的数据库查询模板
/// </summary>
public static class QueryDefine
{
    //参数数：1 0 3 2
    public const string CREATE_TABLE_BEGIN = "CREATE TABLE IF NOT EXISTS {0}(id integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Data TEXT";    //模拟NoSQL
    public const string CREATE_TABLE_END = ");";
    public const string INSERT_WITH_UPDATE = "INSERT OR REPLACE INTO {0} VALUES({1},'{2}');";
    public const string SELECT_PLAYER_DATA = "select Data from {0} where id = {1};";
}

public interface IDataBase
{
    void Init();
    Player GetUserData(int id);
    void SavePlayerData(Player playerData);
    void Close();
}
