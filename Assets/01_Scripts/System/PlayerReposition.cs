using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerReposition : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "AutoFarming")
        {
            Transform playerTransform = Player.Instance.transform;

            Vector3 rePosition = new Vector3(7, 0, 0);
            playerTransform.position = rePosition;
        }
        else
        {
            Transform playerTransform = Player.Instance.transform;

            Vector3 rePosition = new Vector3(0, 0, 0);
            playerTransform.position = rePosition;
        }
    }
}
