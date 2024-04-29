using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    int levelValue;
    public void SwitchScene(int value)
    {
        SceneManager.LoadScene("AutoFarming");

        SceneManager.sceneLoaded += OnSceneLoaded;

        levelValue = value;
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
    }
}
