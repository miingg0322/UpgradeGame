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
        SignupConvenience();
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
            feedbackTextNick.text = "�ߺ��� �г����Դϴ�.";
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
            feedbackTextId.text = "�ߺ��� ���̵��Դϴ�.";
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
            feedbackTextPw.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
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

        // �Է°� ����
        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            feedbackText.text = "��� �׸��� ä���ּ���.";
            return;
        }

        if (!isPwSame)
        {
            feedbackText.text = "��й�ȣ�� ��ġ�ϰ� �Է����ּ���.";
            return;
        }

        if (isIdDuplicate || isNickDuplicate)
        {
            feedbackText.text = "���̵�� �г����� �ߺ����� �ʰ� ������մϴ�.";
            return;
        }

        // DBManager�� ���� ȸ������ ó��
        Init();
        string encryptedPw = Encryptor.SHA256Encryt(password);
        DBManager.Instance.RegisterUser(nickname, id, encryptedPw);
        LoginUi.Instance.ActiveSignUpNotice();
    }

    public void Init()
    {
        nicknameField.text = "";
        idField.text = "";
        passwordField.text = "";
        confirmPasswordField.text = "";
        feedbackText.text = "";
    }

    private void SignupConvenience()
    {
        if (nicknameField.isFocused == true)
        {
            nicknameField.placeholder.GetComponent<TextMeshProUGUI>().text = "";
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                idField.Select();
            }
        }
        else if (idField.isFocused == true)
        {
            idField.placeholder.GetComponent<TextMeshProUGUI>().text = "";
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                passwordField.Select();
            }
        }
        else if (passwordField.isFocused == true)
        {
            passwordField.placeholder.GetComponent<TextMeshProUGUI>().text = "";
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                confirmPasswordField.Select();
            }
        }
        else if (confirmPasswordField.isFocused == true)
        {
            confirmPasswordField.placeholder.GetComponent<TextMeshProUGUI>().text = "";
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                nicknameField.Select();
            }
        }
    }
}
