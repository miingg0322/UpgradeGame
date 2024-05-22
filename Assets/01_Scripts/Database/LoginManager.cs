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
        LoginConvenience();
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

        idField.text = "";
        passwordField.text = "";

        DBManager.Instance.Login(id, password);
    }

    void LoginConvenience()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnLoginButtonClicked();
        }
        else if (idField.isFocused == true)
        {
            //idField.placeholder.GetComponent<TextMeshProUGUI>().text = "";
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                passwordField.Select();
            }
        }
        else if (passwordField.isFocused == true)
        {
            //passwordField.placeholder.GetComponent<TextMeshProUGUI>().text = "";
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                idField.Select();
            }
        }
    }
}
