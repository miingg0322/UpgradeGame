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
    public float maxHealth;
    public float curHealth;
    public float speed;
    public float drainRate;

    public Vector2 inputVec;
    public Scanner scanner;
    public Weapon weapon;
    public Sprite sprite;

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
        Init(GameManager.Instance.selectedClass);     
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

        maxHealth = data.maxHp;
        curHealth = data.curHp;
        speed = data.moveSpeed;
        drainRate = data.drainRate;
        spriter.sprite = data.playerSprite;
    }
}
