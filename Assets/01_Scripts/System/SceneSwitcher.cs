using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class SceneSwitcher : MonoBehaviour
{
    int levelValue;
    string transUserId;
    public void SwitchAutoFarmingScene(int value)
    {
        if (GameManager.Instance.dungeonTicket > 0)
        {
            GameManager.Instance.dungeonTicket--;
            Debug.Log("������ �����Ͽ����ϴ�. ���� ����� ����: " + GameManager.Instance.dungeonTicket);

            SceneManager.LoadScene("AutoFarming");

            SceneManager.sceneLoaded += OnSceneLoaded;

            levelValue = value;

        }
        else
        {
            Debug.Log("���� ������� �����մϴ�.");
        }       
    }

    public void SwitchSampleScene()
    {
        SceneManager.LoadScene("SampleScene");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void GiveUpSwitchSampleScene()
    {
        SceneManager.LoadScene("SampleScene");

        GameManager.Instance.Init();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ReLoadAutoFarmingScene()
    {
        if (GameManager.Instance.dungeonTicket > 0)
        {
            GameManager.Instance.dungeonTicket--;
            Debug.Log("������ �����Ͽ����ϴ�. ���� ����� ����: " + GameManager.Instance.dungeonTicket);

            SceneManager.LoadScene("AutoFarming");

            SceneManager.sceneLoaded += OnSceneLoaded;

            levelValue = GameManager.Instance.DungeonLevel;
        }
        else
        {
            Debug.Log("���� ������� �����մϴ�.");
        }
    }

    public void ReturnCharacterSelect()
    {
        SceneManager.LoadScene("Login");

        SceneManager.sceneLoaded += OnSceneLoaded;

        transUserId = GameManager.Instance.userId;
      
        GameManager.Instance.ReturnChSelect();
        Destroy(GameManager.Instance.player.gameObject);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "AutoFarming")
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.DungeonLevel = levelValue;
            }
            else
            {
                Debug.LogError("GameManager�� ã�� �� �����ϴ�.");
            }
        }
        else if(scene.name == "SampleScene")
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                Time.timeScale = 1f;              
            }
            else
            {
                Debug.LogError("GameManager�� ã�� �� �����ϴ�.");
            }
        }
        else if (scene.name == "Login")
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.userId = transUserId;
            }
            else
            {
                Debug.LogError("GameManager�� ã�� �� �����ϴ�.");
            }
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
