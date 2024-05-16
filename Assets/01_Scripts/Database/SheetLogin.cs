using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SheetLogin : MonoBehaviour
{
    public TMP_InputField idField;
    public TMP_InputField passwordField;

    public Text feedbackText;
    void Start()
    {
        
    }

    public void OnLoginButtonClicked()
    {
        string id = idField.text;
        string password = passwordField.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "모든 항목을 채워주세요.";
            return;
        }
        string encryptedPw = Encryptor.SHA256Encryt(password);
        Debug.Log($"로그인 정보: {id}, {password},{encryptedPw}");
        SheetManager.Instance.Login(id, encryptedPw);
    }
}
