using UnityEngine;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;

public class SQLiteManager : MonoBehaviour
{
    readonly string inventoryDB = "/Inventory.db";
    readonly string inventoryTable = "Inventory";

    SqliteConnection inventoryConn;
    SqliteCommand dbCommand;
    void Start()
    {
        string connectionString = string.Concat("URI=file:", Application.streamingAssetsPath, inventoryDB);
        inventoryConn = new SqliteConnection(connectionString);
        inventoryConn.Open();
        Item testItem = new Item("기본 강화석", 1, 999);
        StartCoroutine(AddItemToInventoryCo(testItem));
        Item testNewItem = new Item("또 다른 재료", 1, 10);
        StartCoroutine(AddItemToInventoryCo(testNewItem));
        StartCoroutine(UseItemFromInventoryCo(testItem, 5));
    }
    public void ReadSQLiteData(string db, string query)
    {
        string dbname = "/ItemTestDB.db";
        string connectionString = "URI=file:" + Application.streamingAssetsPath + dbname;
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        string tablename = "Items";
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "SELECT * FROM " + tablename;
        IDataReader dataReader = dbCommand.ExecuteReader();
        Debug.Log(dataReader.FieldCount);
        while (dataReader.Read())
        {
            string name = dataReader.GetString(0);
            int type = dataReader.GetInt32(1);
            int max = dataReader.GetInt32(2);
            Debug.Log($"{name}, {type}, {max}");
        }
        dataReader.Close();
    }

    public void ConnectDB(string dbpath)
    {
        //string connectionString = string.Concat("URI=file:", Application.streamingAssetsPath, dbpath);
        //dbConnection = new SqliteConnection(connectionString);
        //dbConnection.Open();
        
    }

    SqliteDataReader SelectQuery(string target, string table, string where)
    {
        string query = $"SELECT {target} FROM {table} {where}";
        Debug.Log($"SELECT Query: {query}");
        SqliteCommand dbCommand = inventoryConn.CreateCommand();
        dbCommand.CommandText = query;
        return dbCommand.ExecuteReader();
    }

    int UpdateQuery(string table, string[] cols, string[] values, string where)
    {
        string query = $"UPDATE {table} SET {cols[0]} = '{values[0]}'";
        for (int i = 1; i < cols.Length; i++)
        {
            query = string.Concat(", ", query, cols[i], " = '", values[i], "'");
        }
        query = string.Concat(query, " ", where);
        Debug.Log($"UPDATE Query: {query}");
        SqliteCommand dbCommand = inventoryConn.CreateCommand();
        dbCommand.CommandText = query;
        return dbCommand.ExecuteNonQuery();
    }
    int InsertQuery(string table, string[] cols, string[] values) 
    {
        string query = $"INSERT INTO {table} ({cols[0]}";
        for (int i = 1; i < cols.Length; i++)
        {
            query = string.Concat(query, ",", cols[i]);
        }
        query = string.Concat(query, ") VALUES ('", values[0], "'");
        for (int i = 1; i < values.Length; i++)
        {
            query = string.Concat(query, ",", values[i]);
        }
        query = string.Concat(query, ")");
        Debug.Log(query);
        SqliteCommand dbCommand = inventoryConn.CreateCommand();
        dbCommand.CommandText = query;
        return dbCommand.ExecuteNonQuery();
    }

    // 아이템 획득
    IEnumerator AddItemToInventoryCo(Item item, int amount = 1)
    {
        SqliteDataReader dataReader;
        yield return dataReader = SelectQuery("Amount", inventoryTable, $"WHERE Item = '{item.name}'");
        int result;
        if(dataReader.HasRows)
        {
            dataReader.Read();
            int curAmount = int.Parse(dataReader.GetString(0));
            Debug.Log(curAmount);
            string[] cols = { "Amount" };
            string[] values = { (curAmount + amount).ToString() };
            string where = $"WHERE Item = '{item.name}'";
            yield return result = UpdateQuery(inventoryTable, cols, values, where);
        }
        else
        {
            string[] cols = { "Item", "Type", "Amount" };
            string[] values = { item.name, item.type.ToString(), amount.ToString() };
            yield return result = InsertQuery(inventoryTable, cols, values);
        }

        if (result > 0)
            Debug.Log("Success");
        else
            Debug.Log("Fail");

    }

    // 아이템 사용
    IEnumerator UseItemFromInventoryCo(Item item, int amount = 1)
    {
        SqliteDataReader dataReader;
        yield return dataReader = SelectQuery("Amount", inventoryTable, $"WHERE Item = '{item.name}'");
        int result;
        if (dataReader.HasRows)
        {
            dataReader.Read();
            int curAmount = int.Parse(dataReader.GetString(0));
            Debug.Log(curAmount);
            string[] cols = { "Amount" };
            string[] values = { (curAmount - amount).ToString() };
            string where = $"WHERE Item = '{item.name}'";
            yield return result = UpdateQuery(inventoryTable, cols, values, where);
        }
        else
        {
            result = -1;
        }

        if (result > 0)
            Debug.Log("Success");
        else
            Debug.Log("Fail");

    }
    // 아이템 변경 (강화 등)


}

public class Item
{
    public string name;
    public int type;
    public int max;

    public Item(string name, int type, int max)
    {
        this.name = name;
        this.type = type;
        this.max = max;
    }
}
