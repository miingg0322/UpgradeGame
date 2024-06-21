using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class SceneSwitcher : MonoBehaviour
{
    public InputActionAsset playerAction;

    int levelValue;
    string transUserId;
    public void SwitchAutoFarmingScene(int value)
    {
        if (GameManager.Instance.dungeonTicket > 0)
        {
            GameManager.Instance.dungeonTicket--;
            GameManager.Instance.SaveData();
            Debug.Log("던전에 입장하였습니다. 남은 입장권 개수: " + GameManager.Instance.dungeonTicket);

            SceneManager.LoadScene("AutoFarming");           

            SceneManager.sceneLoaded += OnSceneLoaded;

            levelValue = value;
            AudioManager.instance.PlayBgm(true);
        }
        else
        {
            AudioManager.instance.PlaySkillSfx(AudioManager.SkillSfx.lackTicket);
            Debug.Log("던전 입장권이 부족합니다.");
        }       
    }

    public void SwitchSampleScene()
    {
        SceneManager.LoadScene("SampleScene");    

        SceneManager.sceneLoaded += OnSceneLoaded; 

        AudioManager.instance.PlayBgm(false);
    }

    public void GiveUpSwitchSampleScene()
    {
        SceneManager.LoadScene("SampleScene");

        GameManager.Instance.Init();       

        SceneManager.sceneLoaded += OnSceneLoaded;

        AudioManager.instance.PlayBgm(false);
    }

    public void ReLoadAutoFarmingScene()
    {
        if (GameManager.Instance.dungeonTicket > 0)
        {
            GameManager.Instance.dungeonTicket--;
            GameManager.Instance.SaveData();
            Debug.Log("던전에 입장하였습니다. 남은 입장권 개수: " + GameManager.Instance.dungeonTicket);

            SceneManager.LoadScene("AutoFarming");

            SceneManager.sceneLoaded += OnSceneLoaded;

            levelValue = GameManager.Instance.DungeonLevel;
            AudioManager.instance.PlayBgm(true);
        }
        else
        {
            AudioManager.instance.PlaySkillSfx(AudioManager.SkillSfx.lackTicket);
            Debug.Log("던전 입장권이 부족합니다.");
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

    public void SwitchBossScene()
    {
        GameManager.Instance.SaveData();

        SceneManager.LoadScene("Boss");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SwitchBossToSampleScene()
    {
        SceneManager.LoadScene("SampleScene");

        SceneManager.sceneLoaded += OnSceneLoaded;
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
            // 중복되는 메인 카메라 비활성화
            Player.Instance.transform.GetChild(3).gameObject.SetActive(false);
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
                Debug.LogError("GameManager를 찾을 수 없습니다.");
            }
            // 메인 카메라를 활성화
            Player.Instance.transform.GetChild(3).gameObject.SetActive(true);
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
                Debug.LogError("GameManager를 찾을 수 없습니다.");
            }
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
