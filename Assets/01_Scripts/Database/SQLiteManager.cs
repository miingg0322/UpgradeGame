using UnityEngine;
using System;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Inventory inventory;
    ScrollRect invenView;
    FollowDetail followDetail;
    
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
        SceneManager.sceneLoaded += OnSceneLoaded;

        Connect();

        //StartCoroutine(CreateCharacter(tester));

        //Item testItem = new Item("기본 강화석", 1, 0);
        //StartCoroutine(AddItemToInventoryCo(testItem));

        //Item testNewItem = new Item("또 다른 재료", 1, 0);
        //StartCoroutine(AddItemToInventoryCo(testNewItem));
        //StartCoroutine(UseItemFromInventoryCo(testItem, 5));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if(scene.buildIndex > 0)
        {
            invenView = GameObject.FindGameObjectWithTag("Inventory").GetComponent<ScrollRect>();
            inventory = invenView.GetComponentInChildren<Inventory>();
            followDetail = invenView.GetComponentInChildren<FollowDetail>();
            invenView.gameObject.SetActive(false);
        }
    }

    void Start()
    {

    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex > 0)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (invenView.gameObject.activeSelf)
                {
                    followDetail.gameObject.SetActive(false);
                    invenView.gameObject.SetActive(false);
                }
                else
                {
                    invenView.gameObject.SetActive(true);
                }
            }
        }
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
        string table = $"{inventoryTable}_{SheetManager.Instance.playingCharacter.slot}";
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
        string table = $"{inventoryTable}_{SheetManager.Instance.playingCharacter.slot}";
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
        string table = $"{inventoryTable}_{SheetManager.Instance.playingCharacter.slot}";
        string query = $"INSERT INTO {table} (Item, Type, Amount) VALUES (@Item, @Type, @Amount)";
        Debug.Log(query);
        insertCommand.CommandText = query;
        yield return insertCommand.ExecuteNonQuery();
        insertCommand.Dispose();
    }

    // 아이템 사용
    public void UseItemFromInventory(Item item, int amount = 1)
    {
        StartCoroutine(UseItemFromInventoryCo(item, amount));
    }
    IEnumerator UseItemFromInventoryCo(Item item, int amount = 1)
    {
        SqliteDataReader dataReader;
        string table = $"{inventoryTable}_{SheetManager.Instance.playingCharacter.slot}";
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
    public void UpgradeWeapon(string weaponName, int level)
    {
        StartCoroutine(UpgradeWeaponCo(weaponName, level));
    }
    IEnumerator UpgradeWeaponCo(string weaponName, int level)
    {
        string table = $"{inventoryTable}_{SheetManager.Instance.playingCharacter.slot}";
        SqliteCommand updateCommand = dbConn.CreateCommand();
        updateCommand.Parameters.Add(new SqliteParameter("@Value", level));
        updateCommand.Parameters.Add(new SqliteParameter("@Item", weaponName));
        string query = $"UPDATE {table} SET Value = @Value WHERE Item = @Item";
        //Debug.Log(query);
        updateCommand.CommandText = query;
        yield return updateCommand.ExecuteNonQuery();
        updateCommand.Dispose();

    }
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
        string table = $"{inventoryTable}_{SheetManager.Instance.playingCharacter.slot}";
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
            int value = dataReader.GetInt32(4);
            Item item = new Item(itemName, type, grade, amount, value);
            itemList.Add(item);
        }
        return itemList;
    }

    public Weapon GetEquppiedWeapon()
    {
        SqliteDataReader dataReader;
        string table = $"{inventoryTable}_{SheetManager.Instance.playingCharacter.slot}";
        dataReader = SelectQuery("Item, Grade, Value", table, $"WHERE Equipped = {1}");
        if (dataReader.HasRows)
        {
            dataReader.Read();
            string itemName = dataReader.GetString(0);
            int itemGrade = dataReader.GetInt32(1);
            int level = dataReader.GetInt32(2);
            Weapon weapon = new Weapon();
            weapon.weaponData = Resources.Load<WeaponBaseData>($"/WeaponData/{SheetManager.Instance.playingCharacter.job}/WeaponData_{itemGrade}");
            //$"{Application.streamingAssetsPath}/WeaponData/{playingCharacter.slot}/WeaponData_{itemGrade}";
            return weapon;
        }
        return null;
    }
}

public class Item
{
    public string name;
    public int type;
    public int grade;
    public int amount;
    public int equipped = -1;
    public int value;

    public Item(string name, int type, int grade, int amount = 1 , int value = -1)
    {
        this.name = name;
        this.type = type;
        this.grade = grade;
        this.amount = amount;
        this.value = value;
    }
    public void PrintDetail()
    {
        Debug.Log($"{name}, {type}, {grade}, {amount}, {value}");
    }
}
