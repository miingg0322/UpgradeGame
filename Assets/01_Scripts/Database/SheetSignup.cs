using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

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

        if (!IsValidIdOrPassword(id))
        {
            feedbackText.text = "아이디는 8글자 이상의 영문 + 숫자 조합이어야 합니다.";
            return;
        }

        if (!IsValidIdOrPassword(password))
        {
            feedbackText.text = "비밀번호는 8글자 이상의 영문 + 숫자 조합이어야 합니다.";
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

    private bool IsValidIdOrPassword(string input)
    {
        // 영문과 숫자가 포함된 8글자 이상의 문자열인지 검증
        return Regex.IsMatch(input, @"^(?=.*[a-zA-Z])(?=.*\d).{8,}$");
    }
}
