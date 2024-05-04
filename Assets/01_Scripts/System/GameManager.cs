using Rito;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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

    public GameObject startGroup;
    public GameObject signupGroup;
    public GameObject loginFail;
    public GameObject selectUi;
    public GameObject createUi;
    public GameObject createBtn;

    public Image[] characterIcons;
    public Text[] characterTexts;

    public string userId;
    public int DungeonLevel;

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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActiveSignUp()
    {
        startGroup.SetActive(false);
        signupGroup.SetActive(true);
    }

    public void CancleSignUp()
    {
        startGroup.SetActive(true);
        signupGroup.SetActive(false);
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
            return;

        selectUi.SetActive(false);
        createUi.SetActive(true);
        createBtn.SetActive(false);
    }

    public void CancleCreateCharacter()
    {
        selectUi.SetActive(true);
        createUi.SetActive(false);
        createBtn.SetActive(true);
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

    // ĳ���� ����â���� ������ �������� ��
    public void SelectCharacter(int slotIndex)
    {
        if (userSlots != null)
        {
            // DB���� Characters�� id�� userSlots[slotIndex]�� ���� ã�� ��� �����͸� �������� ���� �ٲٴ� ���� �ۼ�
        }
        else
        {
            Debug.LogError("�����Ͱ� ����ֽ��ϴ�.");
        }
    }

    public void CreateCharacter(int character)
    {
        if (userSlots[userSlots.Length - 1] != 0)
            return;

        string id = userId;
        // DB�� ������ ����
        int characterId = DBManager.Instance.CreateCharacter(id, character);
        
        for(int index = 0;index < userSlots.Length;index++)
        {
            if (userSlots[index] != 0)
            {
                userSlots[index] = characterId;
                break;
            }
        }
        createUi.SetActive(false);
    }
}
