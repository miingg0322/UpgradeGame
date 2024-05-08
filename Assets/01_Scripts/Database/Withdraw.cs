using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Withdraw : MonoBehaviour
{
    public GameObject confirmUi;
    public Button withdrawBtn;
    public TMP_InputField withdrawField;
    public Text withdrawText;
    public Text feedbackText;

    string confirmText;
    private void Start()
    {
        feedbackText.text = "";
    }
    private void Update()
    {
        confirmText = withdrawField.text;

        if(confirmText != withdrawText.text)
        {
            feedbackText.text = "������ ��ġ���� �ʽ��ϴ� �ٽ� �ѹ� Ȯ�����ּ���.";
            withdrawBtn.interactable = false;
        }
        else if(string.IsNullOrEmpty(confirmText))
        {
            feedbackText.text = "";          
        }
        else if(confirmText == withdrawText.text)
        {
            withdrawBtn.interactable = true;
        }
    }
    public void ConfirmWithdraw()
    {
        confirmUi.SetActive(true);
    }
    public void ConfirmWithdrawCancle()
    {
        confirmUi.SetActive(false);
    }
    public void UserWithdraw()
    {
        if (confirmText == withdrawText.text)
        {
            string userId = GameManager.Instance.userId;

            confirmUi.SetActive(false);
            LoginUi.Instance.Logout();
            DBManager.Instance.DeleteUser(userId);
        }     
    }
}
