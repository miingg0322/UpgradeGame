using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    int levelValue;
    int ticketValue;
    public void SwitchAutoFarmingScene(int value)
    {
        if (GameManager.Instance.dungeonTicket.ticket > 0)
        {
            GameManager.Instance.dungeonTicket.ticket--;
            Debug.Log("던전에 입장하였습니다. 남은 입장권 개수: " + GameManager.Instance.dungeonTicket.ticket);
            GameManager.Instance.dungeonTicket.SaveData();

            SceneManager.LoadScene("AutoFarming");

            SceneManager.sceneLoaded += OnSceneLoaded;

            levelValue = value;
        }
        else
        {
            Debug.Log("던전 입장권이 부족합니다.");
        }       
    }

    public void SwitchSampleScene()
    {
        SceneManager.LoadScene("SampleScene");

        SceneManager.sceneLoaded += OnSceneLoaded;

        ticketValue = GameManager.Instance.dungeonTicket.ticket;
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
                Debug.LogError("GameManager를 찾을 수 없습니다.");
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        else if(scene.name == "SampleScene")
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.dungeonTicket.ticket = ticketValue;
                Time.timeScale = 1f;
                GameManager.Instance.dungeonTicket.Init();
            }
            else
            {
                Debug.LogError("GameManager를 찾을 수 없습니다.");
            }
        }
    }
}
