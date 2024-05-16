using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SheetSignup : MonoBehaviour
{
    public TMP_InputField nicknameField;
    public TMP_InputField idField;
    public TMP_InputField passwordField;
    public TMP_InputField confirmPasswordField;

    public Text feedbackTextNick;
    public Text feedbackTextId;
    public Text feedbackTextPw;
    public Text feedbackText;

    void Start()
    {
        
    }

    public void OnSignupButtonClicked()
    {
        string nickname = nicknameField.text;
        string id = idField.text;
        string password = passwordField.text;
        string confirmPassword = confirmPasswordField.text;

        // 입력값 검증
        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            feedbackText.text = "모든 항목을 채워주세요.";
            return;
        }

        if (!password.Equals(confirmPassword))
        {
            feedbackText.text = "비밀번호를 일치하게 입력해주세요.";
            return;
        }

        // DBManager를 통해 회원가입 처리
        string encryptedPw = Encryptor.SHA256Encryt(password);
        SheetManager.Instance.Register(id, encryptedPw, nickname);
    }
}
