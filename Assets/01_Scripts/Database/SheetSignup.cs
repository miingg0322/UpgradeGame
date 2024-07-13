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

        // �Է°� ����
        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            feedbackText.text = "��� �׸��� ä���ּ���.";
            return;
        }

        if (!IsValidIdOrPassword(id))
        {
            feedbackText.text = "���̵�� 8���� �̻��� ���� + ���� �����̾�� �մϴ�.";
            return;
        }

        if (!IsValidIdOrPassword(password))
        {
            feedbackText.text = "��й�ȣ�� 8���� �̻��� ���� + ���� �����̾�� �մϴ�.";
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

    private bool IsValidIdOrPassword(string input)
    {
        // ������ ���ڰ� ���Ե� 8���� �̻��� ���ڿ����� ����
        return Regex.IsMatch(input, @"^(?=.*[a-zA-Z])(?=.*\d).{8,}$");
    }
}
