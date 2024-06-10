using System.Collections;
using System.Collections.Generic;
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
    public float attackSpeed;
    public float drainRate;
    public bool specialMove;

    public WeaponDataManager weaponDataManager;
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

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Boss")
        {
            return; 
        }

        if (Input.GetMouseButtonDown(0))
        {
            // �⺻ ����
            Debug.Log("�⺻ ����");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // �뽬
            Debug.Log("�뽬");
        }
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

        if (!GameManager.Instance.isTutorialClear)
        {
            WeaponBaseData weaponData = weaponDataManager.GetWeaponData(index, 0);
            weapon.weaponData = weaponData;
            weapon.dmg = weaponData.dmgBase;
        }
        else
        {
            // ������ ������ �����͸� �ҷ����� ��������
            WeaponBaseData weaponData = weaponDataManager.GetWeaponData(index, 0);
            weapon.weaponData = weaponData;
            weapon.dmg = weaponData.dmgBase;
        }
        
    }

    // AutoFarming���� ��밡���� �ʻ��
    public void SpecialMove()
    {
        if (specialMove)
        {
            // �ʻ�� ��Ȱ��ȭ
            specialMove = false;
            // �ʻ�� ��ü ����
            specialSkill[playerClass].SetActive(true);
            GameManager.Instance.notice.specialMoveBtn.SetActive(false);
        }           
    }   
}
