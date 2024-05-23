using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoginUi : MonoBehaviour
{
    public SignupManager signupManager;

    public TMP_InputField idField;
    public TMP_InputField passwordField;
    public GameObject loginGroup;
    public GameObject signupGroup;
    public GameObject loginFail;
    public GameObject selectUi;
    public GameObject createUi;
    public GameObject createBtn;
    public GameObject logoutBtn;
    public GameObject chdelBtn;
    public GameObject returnBtn;
    public GameObject delBtn;
    public GameObject signupNotice;
    public GameObject slotNotice;
    public GameObject deleteNotice;
    public GameObject createNotice;
    public GameObject createNoticeCreateBtn;

    public GameObject[] selectBtn;
    public GameObject[] deleteBtn;
    public GameObject[] createChBtn;

    public Image[] selectCharacterIcons;
    public Image[] createCharacterIcons;
    public Text[] characterTexts;

    void Awake()
    {
        GameManager.Instance.AssignLoginUi(this);
    }

    private void Start()
    {
        for(int index = 0; index < selectBtn.Length; index++)
        {
            int selectIndex = index;
            selectBtn[selectIndex].GetComponent<Button>().onClick.AddListener(()=> GameManager.Instance.SelectCharacter(selectIndex));
        }

        for (int index = 0; index < createCharacterIcons.Length; index++)
        {
            int selectIndex = index;
            createCharacterIcons[selectIndex].sprite = GameManager.Instance.playerData[selectIndex].playerSprite;
        }

        chdelBtn.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.DeleteCharacter());
        createNoticeCreateBtn.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.CreateCharacter());
    }
    public void Login()
    {
        loginGroup.SetActive(false);
        selectUi.SetActive(true);
        logoutBtn.SetActive(true);
        createBtn.SetActive(true);

        if(GameManager.Instance.userSlots != null)
            CancleDelete();
    }
    public void ActiveSignUp()
    {
        idField.text = "";
        passwordField.text = "";

        loginGroup.SetActive(false);
        signupGroup.SetActive(true);
    }

    public void CancleSignUp()
    {
        loginGroup.SetActive(true);
        signupGroup.SetActive(false);
        signupManager.Init();
    }

    public void ActiveSignUpNotice()
    {
        signupNotice.SetActive(true);
    }

    public void CancleSignUpNotice()
    {
        signupNotice.SetActive(false);
        signupGroup.SetActive(false);
        loginGroup.SetActive(true);
    }

    public void CancleSlotNotice()
    {
        slotNotice.SetActive(false);
    }
    public void ActiveLoginFail()
    {
        loginFail.SetActive(true);
    }
    public void CancleLoginFail()
    {
        loginFail.SetActive(false);
    }

    public void ActiveDelete()
    {
        int count = 0;

        for(int i = 0; i < GameManager.Instance.userSlots.Length; i++)
        {
            if (GameManager.Instance.userSlots[i] == 0)
                count++;
        }

        if (count == GameManager.Instance.userSlots.Length)
            return;

        for (int index = 0; index < selectBtn.Length; index++)
        {
            if (GameManager.Instance.userSlots[index] != 0)
            {
                selectBtn[index].SetActive(false);
                deleteBtn[index].SetActive(true);
            }          
        }
       
        returnBtn.SetActive(true);
        delBtn.SetActive(false);
    }
    public void CancleDelete()
    {
        for (int index = 0; index < selectBtn.Length; index++)
        {
            if (GameManager.Instance.userSlots[index] != 0)
            {
                selectBtn[index].SetActive(true);
                deleteBtn[index].SetActive(false);
            }              
        }

        if (returnBtn.activeSelf)
        {
            returnBtn.SetActive(false);
            delBtn.SetActive(true);
        }          
    }

    public void ActiveCreateCheck(int index)
    {
        GameManager.Instance.selectIndex = index;
        GameManager.Instance.isCreate = true;
        createNotice.SetActive(true);
    }
    public void ActiveDeleteCheck(int index)
    {
        GameManager.Instance.selectIndex = index;
        GameManager.Instance.deleteCharacter = GameManager.Instance.userSlots[index];
        deleteNotice.SetActive(true);
    }

    public void CancleDeleteCheck()
    {
        deleteNotice.SetActive(false);
    }

    public void CancleCreateCheck()
    {
        createNotice.SetActive(false);
    }
    public void ActiveCreateCharacter()
    {
        if (GameManager.Instance.userSlots[GameManager.Instance.userSlots.Length - 1] != 0)
        {
            slotNotice.SetActive(true);
        }
        else
        {
            selectUi.SetActive(false);
            createUi.SetActive(true);
            createBtn.SetActive(false);
        }
    }

    public void CancleCreateCharacter()
    {
        selectUi.SetActive(true);
        createUi.SetActive(false);
        createBtn.SetActive(true);
    }
    public void Logout()
    { 
        selectUi.SetActive(false);
        createUi.SetActive(false);
        logoutBtn.SetActive(false);
        loginGroup.SetActive(true);

        GameManager.Instance.userId = null;
        GameManager.Instance.userSlots = new int[GameManager.Instance.userSlots.Length];
    }
}
