using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Withdraw : MonoBehaviour
{
    public EscUI escUI;
    public GameObject confirmUi;
    public Button withdrawBtn;
    public TMP_InputField withdrawField;
    public TMP_InputField withdrawText;
    public Text feedbackText;

    string confirmText;
    private void Start()
    {
        feedbackText.text = "";
        withdrawBtn.interactable = false;
    }
    public void ConfirmWithdraw()
    {
        if(confirmUi.activeSelf)
        {
            escUI.CloseUI(confirmUi);
        }
        else
        {
            escUI.OpenUI(confirmUi);
        }      
    }
    public void ConfirmWithdrawCancle()
    {
        escUI.CloseUI(confirmUi);
    }
    public void UserWithdraw()
    {
        if (confirmText == withdrawText.text)
        {
            string userId = GameManager.Instance.userId;

            confirmUi.SetActive(false);
            GameManager.Instance.UserLogout();
            DBManager.Instance.DeleteUser(userId);
        }     
    }
    public void ConfirmInputField()
    {
        confirmText = withdrawField.text;

        if (string.IsNullOrEmpty(confirmText))
        {
            feedbackText.text = "";
            withdrawBtn.interactable = false;
        }
        if (confirmText != withdrawText.text)
        {
            feedbackText.text = "문구가 일치하지 않습니다 다시 한번 확인해주세요.";
            withdrawBtn.interactable = false;
        }
        if (confirmText == withdrawText.text)
        {
            withdrawBtn.interactable = true;
            feedbackText.text = "";
        }
    }
}
