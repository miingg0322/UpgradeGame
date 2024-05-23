using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    public PlayerData[] playerDatas;
    public int playerId;
    public int playerClass;
    public string playerName;
    public string playerTrait;
    public float maxHealth;
    public float curHealth;
    public float speed;
    public float attackSpeed;
    public float drainRate;
    public bool specialMove;

    public Vector2 inputVec;
    public Scanner scanner;
    public Weapon weapon;
    public Sprite sprite;
    public GameObject[] specialSkill;
    

    Rigidbody2D rigid;
    SpriteRenderer spriter;

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

        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        GameManager.Instance.AssignPlayer(this);
        Init(GameManager.Instance.selectedCharacterClass);
    }
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }


    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    public void Init(int index)
    {
        PlayerData data = playerDatas[index];

        playerId = data.playerId;
        playerClass = data.playerId;
        playerName = data.playerName;
        playerTrait = data.playerTrait;

        maxHealth = data.maxHp;
        curHealth = data.curHp;
        speed = data.moveSpeed;
        attackSpeed = data.attackSpeed;
        drainRate = data.drainRate;
        sprite = data.playerSprite;
        spriter.sprite = data.playerSprite;
    }

    // AutoFarming에서 사용가능한 필살기
    public void SpecialMove()
    {
        if (specialMove)
        {
            // 필살기 비활성화
            specialMove = false;
            // 필살기 객체 생성
            specialSkill[playerClass].SetActive(true);
            GameManager.Instance.notice.specialMoveBtn.SetActive(false);
        }           
    }   
}
