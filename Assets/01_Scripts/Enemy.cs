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
    bool isDead;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;
    WaitForSeconds knockBackWait;
    WaitForSeconds deadWait;
    Notice notice;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
        knockBackWait = new WaitForSeconds(0.05f);
        deadWait = new WaitForSeconds(2f);
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
        isDead = false;
    }

    void Update()
    {
        if (!isHit && !isDead)
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        else if (isHit)
            transform.Translate(direction * moveSpeed * -0.4f * Time.deltaTime);
        else if (isDead)
            transform.Translate(direction * 0);
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
                // �ǰ� �ִϸ��̼�, ȿ���� ���
            }
            else
            {
                StartCoroutine(Dead());
                DropItem();              
            }
        }
        else if(collision.CompareTag("Melee"))
        {
            hp -= maxHp;
            StartCoroutine(Dead());
            DropItem();           
        }
    }

    IEnumerator KnockBack()
    {       
            yield return wait;
            isHit = true;

            yield return knockBackWait;
            isHit = false;            
    }

    IEnumerator Dead()
    {
        coll.enabled = false;
        rigid.simulated = false;
        spriter.sortingOrder = 0;
        isDead = true;

        yield return deadWait;

        gameObject.SetActive(false);
    }

    void DropItem()
    {
        if (!(GameManager.Instance.ranItem.GetRandomPick() == "null"))
        {
            string itemName = GameManager.Instance.ranItem.GetRandomPick();

            GameObject ranDropItem = GameManager.Instance.pool.Get(2);
            ranDropItem.transform.position = transform.position;
            DropItem dropItem = ranDropItem.GetComponent<DropItem>();
            dropItem.ReadItemInfo(itemName);
            Sprite sprite = dropItem.GetComponent<SpriteRenderer>().sprite;

            if (!gameObject.activeSelf)
            {
                StartCoroutine(ActivateAndNotice(itemName, sprite));
            }
            else
            {
                NoticeItem(itemName, sprite);
            }          
        }
    }

    void NoticeItem(string itemName, Sprite sprite)
    {
        if(itemName.Contains("�ֹ���") || itemName.Contains("��ȭ��"))
        {
            StartCoroutine(GameManager.Instance.notice.NoticeRoutine());
            GameManager.Instance.notice.noticeText.text = itemName + "�� ȹ���߽��ϴ�!";
            GameManager.Instance.notice.noticeIcon.sprite = sprite;
        }
        else
        {
            return;
        }
    }

    IEnumerator ActivateAndNotice(string itemName, Sprite sprite)
    {
        gameObject.SetActive(true);
        yield return null; 

        NoticeItem(itemName, sprite);

        gameObject.SetActive(false);
    }
}
