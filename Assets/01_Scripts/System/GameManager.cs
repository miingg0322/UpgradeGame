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
    public Weapon weapon;
    public LoginUi loginUi;
    public DungeonTicket dungeonTicket;

    public string userId;
    public int DungeonLevel;
    public int selectedCharacterId;
    public int selectIndex;
    public int deleteCharacter;

    public int[] userSlots;

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

    public void SetUserSlots(int[] slots)
    {
        userSlots = slots;

        for(int index = 0; index < userSlots.Length; index++)
        {
            if (userSlots[index] == 0)
            {
                loginUi.characterIcons[index].sprite = null;
                loginUi.characterTexts[index].text = "Empty";
                loginUi.selectBtn[index].SetActive(false);
            }
            else
            {
               int chClass = DBManager.Instance.GetCharacterClass(userSlots[index]);

                loginUi.characterIcons[index].sprite = playerData[chClass].playerSprite;
                loginUi.characterTexts[index].text = playerData[chClass].playerName;
                loginUi.selectBtn[index].SetActive(true);
            }
        }
    }

    public void SelectCharacter(int slotIndex)
    {
        selectedCharacterId = DBManager.Instance.GetCharacterClass(userSlots[slotIndex]);
        SceneManager.LoadScene("SampleScene");

        StartCoroutine(TicketInit());
    }

    IEnumerator TicketInit()
    {
        yield return null;

        dungeonTicket.Init();
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
        loginUi.createUi.SetActive(false);
        loginUi.selectUi.SetActive(true);
        loginUi.createBtn.SetActive(true);
    }

    public void DeleteCharacter()
    {
        DBManager.Instance.DeleteCharacter(deleteCharacter);
        loginUi.deleteNotice.SetActive(false);
        loginUi.CancleDelete();

        userSlots[selectIndex] = 0;
        SetUserSlots(userSlots);
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

    public void AssignTicket(DungeonTicket ticket)
    {
        dungeonTicket = ticket;
    }
}
