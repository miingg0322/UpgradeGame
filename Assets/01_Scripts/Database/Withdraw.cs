using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        withdrawBtn.interactable = false;
    }
    public void ConfirmWithdraw()
    {
        if(confirmUi.activeSelf)
        {
            confirmUi.SetActive(false);
        }
        else
        {
            confirmUi.SetActive(true);
        }      
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
            UserLogout();
            DBManager.Instance.DeleteUser(userId);
        }     
    }
    public void UserLogout()
    {
        SceneManager.LoadScene("Login");
        Destroy(GameManager.Instance.player.gameObject);

        GameManager.Instance.userId = null;
        GameManager.Instance.userSlots = null;
        GameManager.Instance.selectedCharacterId = -1;
        GameManager.Instance.isCharacterSelect = false;
        GameManager.Instance.SaveData();
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
