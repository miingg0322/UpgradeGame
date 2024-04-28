using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public GameObject dropItem;
    public EnemyData[] enemyData;
    public int hp;
    private int maxHp;
    public int dmg;

    public Vector2 direction;
    public float moveSpeed = 2f;

    bool isHit;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void Start()
    {
        direction = Vector2.right;
    }

    private void OnEnable()
    {
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        hp = maxHp;
        isHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHit)
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        else
            transform.Translate(direction * moveSpeed * -0.4f * Time.deltaTime);
    }

    public void Init(int level)
    {
        EnemyData Data = enemyData[level];

        moveSpeed = Data.moveSpeed;
        maxHp = Data.maxHp;
        hp = Data.curHp;
        spriter.sprite = Data.enemySprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Range"))
        {
            hp -= collision.GetComponent<Range>().damage;
            if (gameObject.activeSelf)
                StartCoroutine(KnockBack());
                
            if(hp > 0)
            {
                // 피격 애니메이션, 효과음 재생
            }
            else
            {
                Dead();
                if (!(GameManager.Instance.ranItem.GetRandomPick() == "null"))
                {
                    GameObject ranDropItem = GameManager.Instance.pool.Get(2);
                    ranDropItem.transform.position = transform.position;
                    DropItem dropItem = ranDropItem.GetComponent<DropItem>();
                    dropItem.ReadItemInfo(GameManager.Instance.ranItem.GetRandomPick());
                }
            }
        }
        else if(collision.CompareTag("Melee"))
        {
            hp -= maxHp;
            Dead();
            if (!(GameManager.Instance.ranItem.GetRandomPick() == "null"))
            {
                GameObject ranDropItem = GameManager.Instance.pool.Get(2);
                ranDropItem.transform.position = transform.position;
                DropItem dropItem = ranDropItem.GetComponent<DropItem>();
                dropItem.ReadItemInfo(GameManager.Instance.ranItem.GetRandomPick());
            }
        }
    }

    IEnumerator KnockBack()
    {       
            yield return wait;
            isHit = true;

            yield return new WaitForSeconds(0.3f);
            isHit = false;            
    }

    void Dead()
    {
        coll.enabled = false;
        rigid.simulated = false;
        spriter.sortingOrder = 0;
        gameObject.SetActive(false);
    }
}
