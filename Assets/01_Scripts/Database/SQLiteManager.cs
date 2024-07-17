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
    readonly static string dbName = "/Player.db";
    readonly string inventoryTable = "Inventory";
    readonly string characterTable = "Character";
    readonly string connectionString = $"data source={Application.streamingAssetsPath}{dbName}";
    public Inventory inventory;
    public ScrollRect invenView;
    FollowDetail followDetail;
    public ItemList itemList;
    public bool isActiveInven;

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
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if(scene.buildIndex > 0)
        {
            invenView = GameObject.FindGameObjectWithTag("Inventory").GetComponent<ScrollRect>();
            inventory = invenView.GetComponentInChildren<Inventory>();
            followDetail = invenView.GetComponentInChildren<FollowDetail>();
            //invenView.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        itemList = GetComponent<ItemList>();
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex > 0)
        {
            if (Input.GetKeyDown(KeySetting.keys[KeyAction.INVENTORY]) && !GameManager.Instance.isStop && !GameManager.Instance.isDungeonClear)
            {
                if (invenView.gameObject.activeSelf)
                {
                    followDetail.gameObject.SetActive(false);
                    invenView.gameObject.SetActive(false);
                    StartCoroutine(InvenDisable());
                }
                else
                {
                    invenView.gameObject.SetActive(true);
                    isActiveInven = true;
                }
            }
            if(Input.GetKeyDown(KeyCode.Escape) && invenView.gameObject.activeSelf)
            {
                followDetail.gameObject.SetActive(false);
                invenView.gameObject.SetActive(false);
                StartCoroutine(InvenDisable());
            }
        }
    }

    SqliteDataReader SelectQuery(string target, string table, string where)
    {
        using(SqliteConnection conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            string query = $"SELECT {target} FROM {table} {where}";
            using (SqliteCommand dbCommand = new SqliteCommand(query, conn))
            {
                dbCommand.CommandText = query;
                return dbCommand.ExecuteReader();
            }
        }
    }

    // 아이템 획득
    //IEnumerator AddItemToInventoryCo(Item item, int amount = 1)
    //{
    //    SqliteDataReader dataReader;
    //    SqliteCommand dbCommand;
    //    dbCommand = dbConn.CreateCommand();
    //    string table = $"{inventoryTable}_{SheetManager.Instance.playingCharacter.slot}";
    //    yield return dataReader = SelectQuery("Amount", table, $"WHERE Item = '{item.name}'");

    //    if (dataReader.HasRows)
    //    {
    //        dataReader.Read();
    //        int curAmount = dataReader.GetInt32(0);
    //        //Debug.Log($"추가 전 {curAmount} 개 있음");
    //        StartCoroutine(UpdateExistItemAmount(item.name, curAmount + amount));
    //    }
    //    else
    //    {
    //        StartCoroutine(InsertNewItem(item, amount));
    //    }

    //}

    IEnumerator InvenDisable()
    {
        yield return null;

        isActiveInven = false;
    }
    IEnumerator UpdateExistItemAmount(string item, int amount)
    {
        using (SqliteConnection conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            string table = $"{inventoryTable}_{SheetManager.Instance.playingCharacter.slot}";
            string query = $"UPDATE {table} SET Amount = @Amount WHERE Item = @Item";
            using (SqliteCommand updateCommand = new SqliteCommand(query, conn))
            {
                updateCommand.Parameters.Add(new SqliteParameter("@Amount", amount));
                updateCommand.Parameters.Add(new SqliteParameter("@Item", item));
                updateCommand.CommandText = query;
                yield return updateCommand.ExecuteNonQuery();
                updateCommand.Dispose();
                conn.Close();
            }
        }
    }
    IEnumerator InsertNewItem(Item item, int amount)
    {
        using (SqliteConnection conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            string table = $"{inventoryTable}_{SheetManager.Instance.playingCharacter.slot}";
            string query = $"INSERT INTO {table} (Item, Type, Amount) VALUES (@Item, @Type, @Amount)";
            using (SqliteCommand insertCommand = new SqliteCommand(query, conn))
            {
                insertCommand.Parameters.Add(new SqliteParameter("@Amount", amount));
                insertCommand.Parameters.Add(new SqliteParameter("@Item", item.name));
                insertCommand.Parameters.Add(new SqliteParameter("@Type", item.type));

                insertCommand.CommandText = query;
                yield return insertCommand.ExecuteNonQuery();
                insertCommand.Dispose();
                conn.Close();
            }
        }
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
        using (SqliteConnection conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            string table = $"{inventoryTable}_{SheetManager.Instance.playingCharacter.slot}";
            string query = $"UPDATE {table} SET Value = @Value WHERE Item = @Item";
            using(SqliteCommand updateCommand = new SqliteCommand(query, conn))
            {
                updateCommand.Parameters.Add(new SqliteParameter("@Value", level));
                updateCommand.Parameters.Add(new SqliteParameter("@Item", weaponName));
                //Debug.Log(query);
                updateCommand.CommandText = query;
                yield return updateCommand.ExecuteNonQuery();
                updateCommand.Dispose();
                conn.Close();
            }
        }
    }
    public Dictionary<Item, int> GetAllItems()
    {
        using (SqliteConnection conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            string table = $"{inventoryTable}_{SheetManager.Instance.playingCharacter.slot}";
            string query = $"SELECT * FROM {table} ORDER BY Type ASC, Grade DESC, Item ASC";
            using(SqliteCommand dbCommand = new SqliteCommand(query, conn))
            {
                dbCommand.CommandText = query;
                SqliteDataReader dataReader = dbCommand.ExecuteReader();
                Dictionary<Item, int> itemDict = new Dictionary<Item, int>();
                while (dataReader.Read())
                {
                    string itemName = dataReader.GetString(0);
                    int type = dataReader.GetInt32(1);
                    int grade = dataReader.GetInt32(2);
                    int amount = dataReader.GetInt32(3);
                    int value = dataReader.GetInt32(4);
                    Item item = new Item(itemList.GetItemData(type, grade));
                    itemDict.Add(item, amount);
                }
                dbCommand.Dispose();
                conn.Close();
                return itemDict;
            }
        }
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

    public void InitInventory(int slot)
    {
        using (SqliteConnection conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            string table = $"{inventoryTable}_{slot}";
            //string query = $"DELETE FROM {table};";
            Item basicWeapon = new Item(itemList.GetItemData(0, 0), 0, 1);
            string query = $"DELETE FROM {table}; INSERT INTO {table} (Item, Type, Grade, Amount, Value, Equipped) VALUES (\"{basicWeapon.name}\", {basicWeapon.type}, {basicWeapon.grade}, {1}, {0}, {1})";
            Debug.Log(query);            
            using(SqliteCommand initCommand = new SqliteCommand(query, conn))
            {
                initCommand.CommandText = query;
                initCommand.ExecuteNonQuery();
                initCommand.Dispose();
            }
        }
    }

    public void SaveItems(Item[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            //StartCoroutine(AddItemToInventoryCo())
        }
    }
}
