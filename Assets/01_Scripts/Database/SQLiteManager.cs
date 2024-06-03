using UnityEngine;
using System;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;

public class SQLiteManager : MonoBehaviour
{
    private static SQLiteManager instance;
    public static SQLiteManager Instance
    {
        get { return instance; }
    }
    readonly string dbName = "/Player.db";
    readonly string inventoryTable = "Inventory";
    readonly string characterTable = "Character";

    SqliteConnection dbConn;

    public Character playingCharacter;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        Character tester = new Character(0, 0);
        playingCharacter = tester;
        Connect();

        //StartCoroutine(CreateCharacter(tester));

        Item testItem = new Item("�⺻ ��ȭ��", 1, 0);
        StartCoroutine(AddItemToInventoryCo(testItem));

        Item testNewItem = new Item("�� �ٸ� ���", 1, 0);
        StartCoroutine(AddItemToInventoryCo(testNewItem));
        StartCoroutine(UseItemFromInventoryCo(testItem, 5));
    }
    void Start()
    {

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

    // ������ ȹ��
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
            //Debug.Log($"�߰� �� {curAmount} �� ����");
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

    // ������ ���
    IEnumerator UseItemFromInventoryCo(Item item, int amount = 1)
    {
        SqliteDataReader dataReader;
        string table = $"{inventoryTable}_{playingCharacter.slot}";
        yield return dataReader = SelectQuery("Amount", table, $"WHERE Item = '{item.name}'");
        if (dataReader.HasRows)
        {
            dataReader.Read();
            int curAmount = dataReader.GetInt32(0);
            //Debug.Log($"��� �� {curAmount} �� ����");
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
    // ������ ���� (��ȭ ��)

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


    public List<Item> GetAllItems()
    {
        string table = $"{inventoryTable}_{playingCharacter.slot}";
        string query = $"SELECT * FROM {table} ORDER BY Type ASC, Grade DESC, Item ASC";
        SqliteCommand dbCommand = dbConn.CreateCommand();
        dbCommand.CommandText = query;
        SqliteDataReader dataReader = dbCommand.ExecuteReader();
        List<Item> itemList = new List<Item>();
        while (dataReader.Read())
        {
            string itemName = dataReader.GetString(0);
            int type = dataReader.GetInt32(1);
            int grade = dataReader.GetInt32(2);
            int amount = dataReader.GetInt32(3);
            Item item = new Item(itemName, type, grade, amount);
            itemList.Add(item);
        }
        return itemList;
    }
}

public class Item
{
    public string name;
    public int type;
    public int grade;
    public int amount;

    public Item(string name, int type, int grade, int amount = 1)
    {
        this.name = name;
        this.type = type;
        this.grade = grade;
        this.amount = amount;
    }
}