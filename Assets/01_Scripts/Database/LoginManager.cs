using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField idField;
    public TMP_InputField passwordField;

    public Text feedbackText;
    private void Start()
    {
        feedbackText.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnLoginButtonClicked();
        }
        else if (idField.isFocused == true)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                passwordField.Select();
            }
        }
        else if (passwordField.isFocused == true)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                idField.Select();
            }
        }
    }


    public void OnLoginButtonClicked()
    {
        string id = idField.text;
        string password = passwordField.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "葛电 亲格阑 盲况林技夸.";
            return;
        }

        DBManager.Instance.Login(id, password);
    }
}
