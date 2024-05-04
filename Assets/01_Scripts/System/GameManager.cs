using Rito;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    public PlayerData[] playerData;
    public PoolManager pool;
    public Player player;
    public RandomItem ranItem;
    public Notice notice;
    public SignupManager signupManager;

    public GameObject loginGroup;
    public GameObject signupGroup;
    public GameObject loginFail;
    public GameObject selectUi;
    public GameObject createUi;
    public GameObject createBtn;
    public GameObject signupNotice;
    public GameObject slotNotice;

    public Image[] characterIcons;
    public Text[] characterTexts;

    public string userId;
    public int DungeonLevel;
    public int selectedClass;

    int[] userSlots;  
    /// <summary>
    /// Key = Upgrade, Cost, Destroy
    /// </summary>
    public Dictionary<string, List<int[]>> dataTables = new Dictionary<string, List<int[]>>();
    public int probBase = 1000;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Login()
    {
        loginGroup.SetActive(false);
        selectUi.SetActive(true);
    }
    public void ActiveSignUp()
    {
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

    public void ActiveCreateCharacter()
    {
        if (userSlots[userSlots.Length - 1] != 0)
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
        loginGroup.SetActive(true);

        userId = null;
        userSlots = new int[userSlots.Length];
    }

    public void SetUserSlots(int[] slots)
    {
        userSlots = slots;

        for(int index = 0; index < userSlots.Length; index++)
        {
            if (userSlots[index] == 0)
            {
                characterIcons[index].sprite = null;
                characterTexts[index].text = "Empty";
            }
            else
            {
               int chClass = DBManager.Instance.GetCharacterClass(userSlots[index]);

                characterIcons[index].sprite = playerData[chClass].playerSprite;
                characterTexts[index].text = playerData[chClass].playerName;
            }
        }
    }

    public void SelectCharacter(int slotIndex)
    {
        if (userSlots != null)
        {
            selectedClass = DBManager.Instance.GetCharacterClass(userSlots[slotIndex]);
            SceneManager.LoadScene("SampleScene");        
        }
        else
        {
            Debug.LogError("�����Ͱ� ����ֽ��ϴ�.");
        }
    }

    public void CreateCharacter(int character)
    {
        if (userSlots[userSlots.Length - 1] != 0)
        {
            return;
        }
            
        string id = userId;
        // DB�� ������ ����
        int characterId = DBManager.Instance.CreateCharacter(id, character);
        
        for(int index = 0;index < userSlots.Length;index++)
        {
            if (userSlots[index] == 0)
            {
                userSlots[index] = characterId;
                break;
            }
        }

        SetUserSlots(userSlots);
        createUi.SetActive(false);
        selectUi.SetActive(true);
        createBtn.SetActive(true);
    }

    public void AssignPlayer(Player playerRef)
    {
        player = playerRef;
    }
}
