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
    public void OnLoginButtonClicked()
    {
        string id = idField.text;
        string password = passwordField.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "��� �׸��� ä���ּ���.";
            return;
        }

        DBManager.Instance.Login(id, password);
    }
}
