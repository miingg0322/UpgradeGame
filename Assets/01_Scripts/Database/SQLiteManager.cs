using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class SQLiteManager : MonoBehaviour
{

    void Start()
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

}
