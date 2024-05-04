using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.SceneManagement;
public class DBManager : MonoBehaviour
{
    private static DBManager instance;
    public static DBManager Instance { get { return instance; } }

    private MySqlConnection connection;
    private string server = DBConfig.dbServer;
    private string database = DBConfig.dbName;
    private string uid = DBConfig.dbId;
    private string password = DBConfig.dbPw;

    private string connectionString;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        connectionString = $"Server={server};Database={database};Uid={uid};Pwd={password};charset=utf8;";
        connection = new MySqlConnection(connectionString);
    }

    public void OpenConnection()
    {
        if (connection.State == System.Data.ConnectionState.Closed)
        {
            connection.Open();
        }
    }

    public void CloseConnection()
    {
        if (connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
        }
    }

    public bool IsNicknameExists(string nickname)
    {
        OpenConnection();

        string query = $"SELECT COUNT(*) FROM users WHERE nickname = '{nickname}'";
        MySqlCommand command = new MySqlCommand(query, connection);
        int count = int.Parse(command.ExecuteScalar().ToString());

        CloseConnection();

        return count > 0;
    }

    public bool IsIdExists(string id)
    {
        OpenConnection();

        string query = $"SELECT COUNT(*) FROM users WHERE id = '{id}'";
        MySqlCommand command = new MySqlCommand(query, connection);
        int count = int.Parse(command.ExecuteScalar().ToString());

        CloseConnection();

        return count > 0;
    }
    public void RegisterUser(string nickname, string id, string password)
    {
        OpenConnection();

        string query = $"INSERT INTO users (nickname, id, pw) VALUES ('{nickname}', '{id}', '{password}')";

        MySqlCommand command = new MySqlCommand(query, connection);
        command.ExecuteNonQuery();

        GameManager.Instance.ActiveSignUpNotice();
        CloseConnection();
    }

    public void Login(string id, string password)
    {
        OpenConnection();

        string encryptedPw = Encryptor.SHA256Encryt(password);
        string query = $"SELECT COUNT(*) FROM users WHERE id = '{id}' AND pw = '{encryptedPw}'";
        MySqlCommand command = new MySqlCommand(query, connection);
        int count = int.Parse(command.ExecuteScalar().ToString());

        CloseConnection();

        if(count > 0)
        {
            GameManager.Instance.SetUserSlots(GetCharacterInfo(id));
            GameManager.Instance.userId = id;
            GameManager.Instance.Login();
        }
        else
        {
            GameManager.Instance.ActiveLoginFail();
        }
    }

    public int[] GetCharacterInfo(string userId)
    {
        OpenConnection();

        int[] slots = new int[3];

        string query = $"SELECT slot1, slot2, slot3 FROM users WHERE id = '{userId}'";
        MySqlCommand command = new MySqlCommand(query, connection);
        MySqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            for (int i = 0; i < 3; i++)
            {
                if (!reader.IsDBNull(i))
                {
                    slots[i] = reader.GetInt32(i);
                }
                else
                {
                    slots[i] = 0; 
                }
            }
        }

        reader.Close();
        CloseConnection();

        for(int i = 0; i < slots.Length; i++)
        {
            Debug.Log(slots[i]);
        }

        return slots;
    }

    public int CreateCharacter(string userId, int characterClass)
    {
        int characterId = -1;

        OpenConnection();

        // 캐릭터 정보를 데이터베이스에 추가합니다.
        string query = $"INSERT INTO characters (class, user_id, clear, inventory, status) VALUES ({characterClass}, '{userId}', 0, 0, 0); SELECT LAST_INSERT_ID();";
        MySqlCommand command = new MySqlCommand(query, connection);
        object result = command.ExecuteScalar();       

        if (result != null && int.TryParse(result.ToString(), out characterId))
        {
            Debug.Log(characterId);
        }
        else
        {
            // 오류 처리
            Debug.LogError("Failed to get character id from database.");
        }

        string updateQuery = $"UPDATE users SET " +
                             $"slot1 = IF(slot1 IS NULL, {characterId}, slot1), " +
                             $"slot2 = IF(slot1 IS NOT NULL AND slot2 IS NULL, IF(slot1 != {characterId}, {characterId}, slot2), slot2), " +
                             $"slot3 = IF(slot1 IS NOT NULL AND slot2 IS NOT NULL AND slot3 IS NULL, IF(slot2 != {characterId}, {characterId}, slot3), slot3) " +
                             $"WHERE id = '{userId}';";
        MySqlCommand updateSlotCommand = new MySqlCommand(updateQuery, connection);
        updateSlotCommand.ExecuteNonQuery();

        CloseConnection();

        return characterId;
    }

    public int GetCharacterClass(int  characterId)
    {
        int characterClass = -1;

        OpenConnection();

        string query = $"SELECT class FROM characters WHERE id = '{characterId}'";
        MySqlCommand command = new MySqlCommand(query, connection);
        MySqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            characterClass = reader.GetInt32(0);
        }

        CloseConnection();
            
        return characterClass;
    }

    public void DeleteCharacter(int characterId)
    {
        string userId = GameManager.Instance.userId;
        OpenConnection();

        string OffQuery = $"SET foreign_key_checks = 0;";
        MySqlCommand offCommand = new MySqlCommand(OffQuery, connection);
        offCommand.ExecuteNonQuery();

        string deleteQuery = $"DELETE FROM characters WHERE id = '{characterId}'";
        MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection);
        deleteCommand.ExecuteNonQuery();

        string OnQuery = $"SET foreign_key_checks = 1;";
        MySqlCommand onCommand = new MySqlCommand(OnQuery, connection);
        onCommand.ExecuteNonQuery();

        if(GameManager.Instance.selectIndex == 0)
        {
            string deleteQuery0 = $"UPDATE users SET slot1 = NULL WHERE id = '{userId}';";
            MySqlCommand deleteCommand0 = new MySqlCommand(deleteQuery0, connection);
            deleteCommand0.ExecuteNonQuery();
        }
        else if(GameManager.Instance.selectIndex == 1)
        {
            string deleteQuery1 = $"UPDATE users SET slot2 = NULL WHERE id = '{userId}';";
            MySqlCommand deleteCommand1 = new MySqlCommand(deleteQuery1, connection);
            deleteCommand1.ExecuteNonQuery();
        }
        else if (GameManager.Instance.selectIndex == 2)
        {
            string deleteQuery2 = $"UPDATE users SET slot3 = NULL WHERE id = '{userId}';";
            MySqlCommand deleteCommand2 = new MySqlCommand(deleteQuery2, connection);
            deleteCommand2.ExecuteNonQuery();
        }
            
        CloseConnection();
    }
}
