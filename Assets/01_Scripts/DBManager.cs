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
    private string server = "127.0.0.1";
    private string database = "testdb";
    private string uid = "root";
    private string password = "wjddn5880@";

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

        GameManager.Instance.CancleSignUp();
        CloseConnection();
    }

    public void Login(string id, string password)
    {
        OpenConnection();

        string query = $"SELECT COUNT(*) FROM users WHERE id = '{id}' AND pw = '{password}'";
        MySqlCommand command = new MySqlCommand(query, connection);
        int count = int.Parse(command.ExecuteScalar().ToString());

        CloseConnection();

        if(count > 0)
        {
            GameManager.Instance.SetUserSlots(GetCharacterInfo(id));
            GameManager.Instance.userId = id;
            SceneManager.LoadScene("CharacterSelect");
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
        SceneManager.LoadScene("CharacterSelect");
        return slots;
    }

    public void CreateCharacter(string userId, int characterClass)
    {
        OpenConnection();

        // 캐릭터 정보를 데이터베이스에 추가합니다.
        string query = $"INSERT INTO characters (class, user_id, clear, inventory, status) VALUES ({characterClass}, '{userId}', 0, 0, 0)";
        MySqlCommand command = new MySqlCommand(query, connection);
        int result = command.ExecuteNonQuery();

        CloseConnection();
    }
}
