using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using System;
using System.Runtime.ConstrainedExecution;
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

        string OffQuery = $"SET foreign_key_checks = 0;";
        MySqlCommand offCommand = new MySqlCommand(OffQuery, connection);
        offCommand.ExecuteNonQuery();

        string query = $"INSERT INTO users (nickname, id, pw, tutorial) VALUES ('{nickname}', '{id}', '{password}', 0)";
        MySqlCommand command = new MySqlCommand(query, connection);
        command.ExecuteNonQuery();

        string OnQuery = $"SET foreign_key_checks = 1;";
        MySqlCommand onCommand = new MySqlCommand(OnQuery, connection);
        onCommand.ExecuteNonQuery();

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
            GameManager.Instance.isTutorialClear = GetTutorialClear(id);
            GameManager.Instance.loginUi.Login();
        }
        else
        {
            GameManager.Instance.loginUi.ActiveLoginFail();
        }
    }

    public int GetClearInfo(int characterId)
    {
        int clear = -1;

        OpenConnection();

        string query = $"SELECT clear FROM characters WHERE id = '{characterId}'";
        MySqlCommand command = new MySqlCommand(query, connection);
        MySqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            clear = reader.GetInt32(0);
        }

        CloseConnection();

        return clear;
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
                    slots[i] = -1; 
                }
            }
        }

        reader.Close();
        CloseConnection();

        return slots;
    }

    public bool GetTutorialClear(string userId)
    {
        OpenConnection();

        int tutorial = -1;

        string query = $"SELECT tutorial FROM users WHERE id = '{userId}'";
        MySqlCommand command = new MySqlCommand(query, connection);
        MySqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            tutorial = reader.GetInt32(0);
        }

        bool tutorialClear = tutorial == 1 ? true : false;

        CloseConnection();

        return tutorialClear;
    }

    public void TutorialClear(string userId)
    {
        OpenConnection();

        string query = $"UPDATE users SET tutorial = 1 WHERE id = '{userId}'";
        MySqlCommand command = new MySqlCommand(query, connection);
        command.ExecuteNonQuery();

        CloseConnection();
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

    public int GetCharacterClass(int  slotIndex)
    {
        int characterClass = -1;

        OpenConnection();

        string query = $"SELECT class FROM characters WHERE id = '{slotIndex}'";
        MySqlCommand command = new MySqlCommand(query, connection);
        MySqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            characterClass = reader.GetInt32(0);
        }

        CloseConnection();
            
        return characterClass;
    }

    public void UpdateBossClear(int characterId, int bossClear)
    {
        OpenConnection();

        string query = $"UPDATE characters SET clear = '{bossClear}' WHERE id = '{characterId}'";
        MySqlCommand command = new MySqlCommand(query, connection);
        command.ExecuteNonQuery();

        Debug.Log(characterId + "번 캐릭터의" + bossClear + "단계 보스 클리어 정보 저장");
        CloseConnection();
    }

    public int GetBossClear(int characterId)
    {
        int clear = -1;

        OpenConnection();

        string query = $"SELECT clear FROM characters WHERE id = '{characterId}'";
        MySqlCommand command = new MySqlCommand(query, connection);
        MySqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            clear = reader.GetInt32(0);
        }

        CloseConnection();

        return clear;
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

    public void DeleteUser(string userId)
    {
        OpenConnection();

        string OffQuery = $"SET foreign_key_checks = 0;";
        MySqlCommand offCommand = new MySqlCommand(OffQuery, connection);
        offCommand.ExecuteNonQuery();

        string deleteQuery = $"DELETE characters, inventory, item FROM users LEFT JOIN characters ON users.id = characters.user_id LEFT JOIN inventory ON characters.id = inventory.character_id LEFT JOIN item ON inventory.item_id = item.id WHERE users.id = '{userId}'";
        MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection);
        Console.WriteLine(deleteCommand.CommandText);
        deleteCommand.ExecuteNonQuery();

        string deleteQuery2 = $"DELETE FROM users WHERE id = '{userId}'";
        MySqlCommand deleteCommand2 = new MySqlCommand(deleteQuery2, connection);
        deleteCommand2.ExecuteNonQuery();

        string OnQuery = $"SET foreign_key_checks = 1;";
        MySqlCommand onCommand = new MySqlCommand(OnQuery, connection);
        onCommand.ExecuteNonQuery();

        CloseConnection();
    }
}
