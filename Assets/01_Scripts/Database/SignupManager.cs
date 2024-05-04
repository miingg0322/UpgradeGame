using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class SignupManager : MonoBehaviour
{
    public TMP_InputField nicknameField;
    public TMP_InputField idField;
    public TMP_InputField passwordField;
    public TMP_InputField confirmPasswordField;
    public Text feedbackTextNick;
    public Text feedbackTextId;
    public Text feedbackTextPw;
    public Text feedbackText;

    bool isPwSame;
    bool isIdDuplicate;
    bool isNickDuplicate;
    private void Start()
    {
        feedbackTextNick.text = "";
        feedbackTextId.text = "";
        feedbackTextPw.text = "";
        feedbackText.text = "";      
    }

    private void Update()
    {
        OnNicknameInputChanged();
        OnIdInputChanged();
        OnPwCheckInputChanged();
    }

    public void OnNicknameInputChanged()
    {
        string nickname = nicknameField.text;

        if (string.IsNullOrEmpty(nickname))
        {
            feedbackTextNick.text = "";
            return;
        }

        if (DBManager.Instance.IsNicknameExists(nickname))
        {
            feedbackTextNick.text = "중복된 닉네임입니다.";
            isNickDuplicate = true;  
        }
        else
        {
            feedbackTextNick.text = "";
            isNickDuplicate = false;
        }
    }

    public void OnIdInputChanged()
    {
        string id = idField.text;

        if (string.IsNullOrEmpty(id))
        {
            feedbackTextId.text = "";
            return;
        }

        if (DBManager.Instance.IsIdExists(id))
        {
            feedbackTextId.text = "중복된 아이디입니다.";
            isIdDuplicate = true;
        }
        else
        {
            feedbackTextId.text = "";
            isIdDuplicate = false;
        }
    }

    public void OnPwCheckInputChanged()
    {
        string password = passwordField.text;
        string confirmPassword = confirmPasswordField.text;

        if(string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            feedbackTextPw.text = "";
            return;
        }
        if (password != confirmPassword)
        {
            feedbackTextPw.text = "비밀번호가 일치하지 않습니다.";
            isPwSame = false;
        }
        else
        {
            feedbackTextPw.text = "";
            isPwSame = true;
        }
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

        if (!isPwSame)
        {
            feedbackText.text = "비밀번호를 일치하게 입력해주세요.";
            return;
        }

        if (isIdDuplicate || isNickDuplicate)
        {
            feedbackText.text = "아이디와 닉네임은 중복되지 않게 지어야합니다.";
            return;
        }

        // DBManager를 통해 회원가입 처리
        string encryptedPw = Encryptor.SHA256Encryt(password);
        DBManager.Instance.RegisterUser(nickname, id, encryptedPw);
    }
}
