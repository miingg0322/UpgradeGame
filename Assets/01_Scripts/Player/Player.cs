using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    public float dashSpeed = 1;
    public float attackSpeed;
    public float drainRate;
    public bool specialMove;

    public WeaponDataManager weaponDataManager;
    public Vector2 movement;
    public Vector2 dashVec;
    public Scanner scanner;
    public Weapon weapon;
    public Sprite sprite;
    public Renderer render;
    public GameObject[] specialSkill;

    float chMoveSpeed;

    bool isDash;
    bool isDashCoolTime;
    bool isDead;

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
        render = GetComponent<Renderer>();
    }

    private void Start()
    {
        GameManager.Instance.AssignPlayer(this);
        Init(GameManager.Instance.selectedCharacterClass);
    }

    private void Update()
    {
        Move();
        Attack();
        Dash();
        Skill();
    }
    //void OnMove(InputValue value)
    //{
    //    inputVec = value.Get<Vector2>();
    //}

    void Move()
    {
        if (!isDash)
        {
            movement = Vector2.zero;

            if (Input.GetKey(KeySetting.keys[KeyAction.UP]))
            {
                movement.y = 1;
            }
            if (Input.GetKey(KeySetting.keys[KeyAction.DOWN]))
            {
                movement.y = -1;
            }
            if (Input.GetKey(KeySetting.keys[KeyAction.LEFT]))
            {
                movement.x = -1;
            }
            if (Input.GetKey(KeySetting.keys[KeyAction.RIGHT]))
            {
                movement.x = 1;
            }

            movement = movement.normalized;
        }                         
    } 

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + movement * speed * dashSpeed * Time.fixedDeltaTime);
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
        chMoveSpeed = data.moveSpeed;
        attackSpeed = data.attackSpeed;
        drainRate = data.drainRate;
        sprite = data.playerSprite;
        spriter.sprite = data.playerSprite;

        if (!GameManager.Instance.isTutorialClear)
        {
            WeaponBaseData weaponData = weaponDataManager.GetWeaponData(index, 0);
            weapon.weaponData = weaponData;
            weapon.dmg = weaponData.dmgBase;
        }
        else
        {
            // 장착한 무기의 데이터를 불러오게 구현예정
            WeaponBaseData weaponData = weaponDataManager.GetWeaponData(index, 0);
            weapon.weaponData = weaponData;
            weapon.dmg = weaponData.dmgBase;
        }
        
    }
    void Attack()
    {
        
    }
    void Dash()
    {
        if (Input.GetKey(KeySetting.keys[KeyAction.DASH]))
        {
            if (movement != Vector2.zero && !isDash && !isDead && !isDashCoolTime)
            {
                dashVec = movement;
                dashSpeed = 4;
                isDash = true;
                isDashCoolTime = true;

                StartCoroutine(DashEnd());
            }
        }      
    }

    void Skill()
    {
        if (Input.GetKey(KeySetting.keys[KeyAction.SKILL]))
        {
            Debug.Log("SKILL");
        }
    }
    IEnumerator DashEnd()
    {
        yield return new WaitForSeconds(0.15f);
        isDash = false;
        dashSpeed = 1;

        yield return new WaitForSeconds(0.85f);
        isDashCoolTime = false;
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
    public void DisableSpecialSkill()
    {
        specialSkill[playerClass].SetActive(false);
    }

}
