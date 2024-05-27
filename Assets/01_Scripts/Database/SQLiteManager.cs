using UnityEngine;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;

public class SQLiteManager : MonoBehaviour
{

    readonly string dbName = "/Player.db";
    private string inventoryTable = "Inventory";
    readonly string characterTable = "Character";

    SqliteConnection dbConn;

    Character playingCharacter;
    void Start()
    {
        Character tester = new Character(0, 0);
        playingCharacter = tester;
        Connect();

        //StartCoroutine(CreateCharacter(tester));

        Item testItem = new Item("기본 강화석", 1, 999);
        StartCoroutine(AddItemToInventoryCo(testItem));

        Item testNewItem = new Item("또 다른 재료", 1, 10);
        StartCoroutine(AddItemToInventoryCo(testNewItem));
        StartCoroutine(UseItemFromInventoryCo(testItem, 5));
    }

    private void Connect()
    {
        string connectionString = $"URI=file:{Application.streamingAssetsPath}{dbName}";
        dbConn = new SqliteConnection(connectionString);
        dbConn.Open();
    }

    SqliteDataReader SelectQuery(string target, string table, string where)
    {
        string query = $"SELECT {target} FROM {table} {where}";
        //Debug.Log($"SELECT Query: {query}");
        SqliteCommand dbCommand = dbConn.CreateCommand();
        dbCommand.CommandText = query;
        return dbCommand.ExecuteReader();
    }

    // 아이템 획득
    IEnumerator AddItemToInventoryCo(Item item, int amount = 1)
    {
        SqliteDataReader dataReader;
        SqliteCommand dbCommand;
        dbCommand = dbConn.CreateCommand();
        string table = $"{inventoryTable}_{playingCharacter.slot}";
        yield return dataReader = SelectQuery("Amount", table, $"WHERE Item = '{item.name}'");

        if(dataReader.HasRows)
        {
            dataReader.Read();
            int curAmount = dataReader.GetInt32(0);
            //Debug.Log($"추가 전 {curAmount} 개 있음");
            StartCoroutine(UpdateExistItemAmount(item.name, curAmount + amount));
        }
        else
        {
            StartCoroutine(InsertNewItem(item, amount));
        }

    }
    IEnumerator UpdateExistItemAmount(string item, int amount)
    {
        SqliteCommand updateCommand = dbConn.CreateCommand();
        updateCommand.Parameters.Add(new SqliteParameter("@Amount", amount));
        updateCommand.Parameters.Add(new SqliteParameter("@Item", item));
        string table = $"{inventoryTable}_{playingCharacter.slot}";
        string query = $"UPDATE {table} SET Amount = @Amount WHERE Item = @Item";
        //Debug.Log(query);
        updateCommand.CommandText = query;
        yield return updateCommand.ExecuteNonQuery();
        updateCommand.Dispose();
    }
    IEnumerator InsertNewItem(Item item, int amount)
    {
        SqliteCommand insertCommand = dbConn.CreateCommand();
        insertCommand.Parameters.Add(new SqliteParameter("@Amount", amount));
        insertCommand.Parameters.Add(new SqliteParameter("@Item", item.name));
        insertCommand.Parameters.Add(new SqliteParameter("@Type", item.type));
        string table = $"{inventoryTable}_{playingCharacter.slot}";
        string query = $"INSERT INTO {table} (Item, Type, Amount) VALUES (@Item, @Type, @Amount)";
        Debug.Log(query);
        insertCommand.CommandText = query;
        yield return insertCommand.ExecuteNonQuery();
        insertCommand.Dispose();
    }

    // 아이템 사용
    IEnumerator UseItemFromInventoryCo(Item item, int amount = 1)
    {
        SqliteDataReader dataReader;
        string table = $"{inventoryTable}_{playingCharacter.slot}";
        yield return dataReader = SelectQuery("Amount", table, $"WHERE Item = '{item.name}'");
        if (dataReader.HasRows)
        {
            dataReader.Read();
            int curAmount = dataReader.GetInt32(0);
            //Debug.Log($"사용 전 {curAmount} 개 있음");
            if (curAmount >= amount)
                StartCoroutine(UpdateExistItemAmount(item.name, curAmount - amount));
            else
                yield return false;
        }
        else
        {
            yield return false;
        }

    }
    // 아이템 변경 (강화 등)

    IEnumerator CreateCharacter(Character character)
    {
        SqliteCommand createCharacterCommand = dbConn.CreateCommand();
        createCharacterCommand.Parameters.Add(new SqliteParameter("@Slot", character.slot));
        createCharacterCommand.Parameters.Add(new SqliteParameter("@Class", (int)character.charClass));
        createCharacterCommand.Parameters.Add(new SqliteParameter("@Created", character.created));
        string query = $"INSERT INTO {characterTable} (Slot, Class, Created) VALUES (@Slot, @Class, @Created)";
        createCharacterCommand.CommandText = query;
        yield return createCharacterCommand.ExecuteNonQuery();
    }

    IEnumerator DeleteCharacter(int slot)
    {
        SqliteCommand createCharacterCommand = dbConn.CreateCommand();
        string query = $"DELETE FROM {characterTable} WHERE Slot = {slot}";
        createCharacterCommand.CommandText = query;
        yield return createCharacterCommand.ExecuteNonQuery();
    }
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
