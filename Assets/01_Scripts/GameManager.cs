using Rito;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    public string userId;
    public int DungeonLevel;

    int[] userSlots;
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
    }

    // 캐릭터 선택창에서 슬룻을 선택했을 때
    public void SelectCharacter(int slotIndex)
    {
        if (userSlots != null)
        {
            // DB에서 Characters의 id가 userSlots[slotIndex]인 것을 찾아 모든 데이터를 가져오고 씬을 바꾸는 로직 작성
        }
        else
        {
            Debug.LogError("데이터가 비어있습니다.");
        }
    }

    public void CreateCharacter(int character)
    {
        string id = userId;
        // DB에 데이터 생성
        DBManager.Instance.CreateCharacter(id, character);
    }
}
