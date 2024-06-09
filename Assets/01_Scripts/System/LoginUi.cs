using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    public GameObject eyeOff;
    public GameObject eyeOn;
    public GameObject capsLockUi;

    public GameObject[] selectBtn;
    public GameObject[] deleteBtn;
    public GameObject[] createChBtn;

    public Image[] selectCharacterIcons;
    public Image[] createCharacterIcons;
    public Text[] characterTexts;

    GameObject activeUi;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && activeUi != null)
        {           
            if (activeUi == signupGroup)
            {
                loginGroup.SetActive(true);
            }
            if (activeUi == signupNotice)
            {
                signupGroup.SetActive(false);
                loginGroup.SetActive(true);
            }
            activeUi.SetActive(false);
        }


        bool isCapsLockOn = IsCapsLockOn();
        capsLockUi.GetComponent<Animator>().SetBool("CapsLockOn", isCapsLockOn);    
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

    // Password InputField에서 아이콘 클릭시 비밀번호가 보여지거나 숨겨지는 함수
    public void SetShowPassword()
    {
        if(eyeOff.activeSelf)
        {
            eyeOff.SetActive(false);
            eyeOn.SetActive(true);
            passwordField.contentType = TMP_InputField.ContentType.Standard;
        }
        else
        {
            eyeOff.SetActive(true);
            eyeOn.SetActive(false);
            passwordField.contentType = TMP_InputField.ContentType.Password;
        }
    }
    public void ActiveSignUp()
    {
        AudioManager.Instance.PlayUISfx(AudioManager.UISfx.uiList);
        idField.text = "";
        passwordField.text = "";

        loginGroup.SetActive(false);
        signupGroup.SetActive(true);
        activeUi = signupGroup;
    }

    public void CancleSignUp()
    {
        loginGroup.SetActive(true);
        signupGroup.SetActive(false);
        signupManager.Init();
        activeUi = null;
    }

    public void ActiveSignUpNotice()
    {
        signupNotice.SetActive(true);
        activeUi = signupNotice;
    }

    public void CancleSignUpNotice()
    {
        signupNotice.SetActive(false);
        signupGroup.SetActive(false);
        loginGroup.SetActive(true);
        activeUi = null;
    }

    public void CancleSlotNotice()
    {
        slotNotice.SetActive(false);
    }
    public void ActiveLoginFail()
    {
        loginFail.SetActive(true);
        activeUi = loginFail;
    }
    public void CancleLoginFail()
    {
        loginFail.SetActive(false);
        activeUi = null;
    }

    public void ActiveDelete()
    {
        int count = 0;

        for(int i = 0; i < GameManager.Instance.userSlots.Length; i++)
        {
            if (GameManager.Instance.userSlots[i] == -1)
                count++;
        }

        if (count == GameManager.Instance.userSlots.Length)
            return;

        for (int index = 0; index < selectBtn.Length; index++)
        {
            if (GameManager.Instance.userSlots[index] != -1)
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
            if (GameManager.Instance.userSlots[index] != -1)
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
        activeUi = createNotice;
    }
    public void ActiveDeleteCheck(int index)
    {
        GameManager.Instance.selectIndex = index;
        GameManager.Instance.deleteCharacter = GameManager.Instance.userSlots[index];
        deleteNotice.SetActive(true);
        activeUi = deleteNotice;
    }

    public void CancleDeleteCheck()
    {
        deleteNotice.SetActive(false);
        activeUi = null;
    }

    public void CancleCreateCheck()
    {
        createNotice.SetActive(false);
        activeUi = null;
    }
    public void ActiveCreateCharacter()
    {
        int count = 0;
        for (int index = 0; index <  GameManager.Instance.userSlots.Length; index++)
        {       
            if (GameManager.Instance.userSlots[index] != -1)
            {
                count++;
            }
        }
        if(count == GameManager.Instance.userSlots.Length)
        {
            slotNotice.SetActive(true);
            activeUi = slotNotice;
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

    bool IsCapsLockOn()
    {
        return (GetKeyState(0x14) & 0xffff) != 0;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern short GetKeyState(int keyCode);
}
