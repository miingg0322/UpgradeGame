using Rito;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
    public Weapon weapon;
    public LoginUi loginUi;
    public SampleSceneUI sampleSceneUI;

    public GameObject cleaner;

    public string userId;
    public int DungeonLevel;   
    public int selectedCharacterClass;
    public int selectedCharacterId;
    public int selectIndex;
    public int deleteCharacter;
    public int bossClear;

    public int dungeonTicket;
    public int maxDungeonTicket = 5;
    public float ticketGenerationTime = 600;
    public float timer;

    public int[] userSlots;

    public List<CollectItem> collectedItems = new List<CollectItem>();

    public bool isDungeonClear;
    public bool isCharacterSelect;
    public bool isCreate;
    public bool isChangeSetting;
    public bool isTutorialClear;

    /// <summary>
    /// Key = Upgrade, Cost, Destroy
    /// </summary>
    public Dictionary<string, List<int[]>> dataTables = new Dictionary<string, List<int[]>>();
    public int probBase = 1000;
    private void Awake()
    {      
        if (instance == null)
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
        if (isCharacterSelect)
        {
            if (dungeonTicket != maxDungeonTicket)
            {
                timer += Time.deltaTime;
            }

            if (timer >= ticketGenerationTime)
            {
                timer = 0f;
                IncreaseTicket();
            }
        }          
    }

    void IncreaseTicket()
    {
        if (dungeonTicket < maxDungeonTicket)
        {
            dungeonTicket++;
            Debug.Log("던전 입장권이 자동으로 생성되었습니다. 현재 입장권 개수: " + dungeonTicket);
        }
    }

    public void Init()
    {
        int characterId = selectedCharacterId;

        int saveTicket = PlayerPrefs.GetInt("Character" + characterId + "Ticket", maxDungeonTicket);
        float saveTimer = PlayerPrefs.GetFloat("Character" + characterId + "Timer", 0);
        long saveTime = Convert.ToInt64(PlayerPrefs.GetString("Character" + characterId + "Time", "0"));

        if (saveTicket >= maxDungeonTicket)
        {
            dungeonTicket = maxDungeonTicket;
            timer = 0f;
            return;
        }
        else
        {
            TimeSpan timePassed = DateTime.Now - new DateTime(saveTime);
            float elapsedTime = (float)timePassed.TotalSeconds;
            int increasedTickets = (int)(elapsedTime / ticketGenerationTime);
            saveTicket += increasedTickets;
            saveTimer += (long)(elapsedTime % ticketGenerationTime);

            if (saveTicket >= maxDungeonTicket)
            {
                saveTicket = maxDungeonTicket;
                saveTimer = 0f;
            }

            dungeonTicket = saveTicket;
            timer = Mathf.Min(saveTimer, ticketGenerationTime);

            Debug.Log("타이머가 초기화 되었습니다");
        }
    }

    public void SaveData()
    {
        int characterId = selectedCharacterId;

        PlayerPrefs.SetInt("Character" + characterId + "Ticket", dungeonTicket);
        PlayerPrefs.SetFloat("Character" + characterId + "Timer", timer);
        PlayerPrefs.SetString("Character" + characterId + "Time", DateTime.Now.Ticks.ToString());

        Debug.Log("데이터가 저장 되었습니다");
    }

    public void SetUserSlots(int[] slots)
    {
        userSlots = slots;

        for(int index = 0; index < userSlots.Length; index++)
        {
            if (userSlots[index] == -1)
            {
                loginUi.selectCharacterIcons[index].sprite = null;
                loginUi.characterTexts[index].text = "Empty";
                loginUi.selectBtn[index].SetActive(false);
            }
            else
            {
                int chClass = slots[index];

                loginUi.selectCharacterIcons[index].sprite = playerData[chClass].playerSprite;
                loginUi.characterTexts[index].text = playerData[chClass].playerName;
                loginUi.selectBtn[index].SetActive(true);
            }
        }
    }

    public void SelectCharacter(int slotIndex)
    {
        selectIndex = slotIndex;
        selectedCharacterClass = userSlots[slotIndex];
        SheetManager.Instance.playingCharacter = SheetManager.Instance.characterDatas[slotIndex];
        bossClear = SheetManager.Instance.characterDatas[slotIndex].clear;
        isTutorialClear = SheetManager.Instance.characterDatas[slotIndex].tutorial;
        SceneManager.LoadScene("SampleScene");

        StartCoroutine(Tutorial());

        isCharacterSelect = true;
        StartCoroutine(TicketInit());
    }

    IEnumerator Tutorial()
    {
        yield return null;

        if (!isTutorialClear)
        {
            sampleSceneUI.tutorialUi.SetActive(true);         
        }
    }
    IEnumerator TicketInit()
    {
        yield return null;

        Init();
    }

    public void CreateCharacter()
    {                 
        int slotIndex = -1;
        for (int i = 0; i < userSlots.Length; i++)
        {
            if (userSlots[i] == -1)
            {
                slotIndex = i;
                break;
            }
        }
        if(slotIndex > -1)
        {
            SheetManager.Instance.CreateCharacter(slotIndex, selectIndex);
            userSlots[slotIndex] = selectIndex;
        }
        SetUserSlots(userSlots);
        loginUi.createUi.SetActive(false);
        loginUi.selectUi.SetActive(true);
        loginUi.createBtn.SetActive(true);
        loginUi.createNotice.SetActive(false);
    }

    public void DeleteCharacter()
    {
        SheetManager.Instance.DeleteCharacter(selectIndex);
        loginUi.deleteNotice.SetActive(false);
        loginUi.CancleDelete();

        userSlots[selectIndex] = -1;
        SetUserSlots(userSlots);
    }

    public void CollectedItemsInit()
    {
        collectedItems = new List<CollectItem>();
    }

    public void CollectItem(string name, string grade, Sprite sprite)
    {
        // 중복된 아이템이 있는지 확인
        foreach (CollectItem item in collectedItems)
        {
            if (item.itemName == name && item.itemGrade == grade)
            {
                // 중복된 아이템이 있으면 수량을 증가시키고 함수 종료
                item.itemQuantity++;
                Debug.Log("중복된 아이템. 수량 증가");
                return;
            }
        }

        CollectItem newItem = new CollectItem(name, grade, sprite, 1);
        collectedItems.Add(newItem);
        collectedItems.Sort(new CollectItemComparer());
    }
    public void ReturnChSelect()
    {
        StartCoroutine(ReturnChSelectRoutine());
    }

    IEnumerator ReturnChSelectRoutine()
    {
        yield return null;

        loginUi.Login();
        SetUserSlots(userSlots);
    }

    // 테스트 구현
    public void BossClear(int bossIndex)
    {
        if (bossClear < bossIndex)
        {
            Debug.Log("캐릭터 ID :" + selectedCharacterId + ", 클리어한 보스 Index :" + bossIndex);
            DBManager.Instance.UpdateBossClear(selectedCharacterId, bossIndex);
            bossClear = bossIndex;
        }     
    }

    public void AssignPlayer(Player playerRef)
    {
        player = playerRef;
    }
    public void AssignPool(PoolManager poolmanager)
    {
        pool = poolmanager;
    }
    public void AssignRanItem(RandomItem randomItem)
    {
        ranItem = randomItem;
    }
    public void AssignNotice(Notice noti)
    {
        notice = noti;
    }

    public void AssignLoginUi(LoginUi login)
    {
        loginUi = login;
    }
    public void AssignSampleSceneUi(SampleSceneUI sampleScene)
    {
        sampleSceneUI = sampleScene;
    }
}

public class CollectItemComparer : IComparer<CollectItem>
{
    public int Compare(CollectItem x, CollectItem y)
    {
        // rare 등급이 List의 앞에 배치되게 정렬
        if (x.itemGrade == "rare" && y.itemGrade != "rare")
            return -1;
        if (x.itemGrade != "rare" && y.itemGrade == "rare")
            return 1;

        // 등급이 같으면 이름 순서로 정렬
        return string.Compare(x.itemName, y.itemName, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.CompareOptions.StringSort);
    }
}
