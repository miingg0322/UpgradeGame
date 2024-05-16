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

        // �Է°� ����
        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            feedbackText.text = "��� �׸��� ä���ּ���.";
            return;
        }

        if (!password.Equals(confirmPassword))
        {
            feedbackText.text = "��й�ȣ�� ��ġ�ϰ� �Է����ּ���.";
            return;
        }

        // DBManager�� ���� ȸ������ ó��
        string encryptedPw = Encryptor.SHA256Encryt(password);
        SheetManager.Instance.Register(id, encryptedPw, nickname);
    }
}
