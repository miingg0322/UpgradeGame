using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public GameObject dropItem;
    public EnemyData[] enemyData;
    public FarmingBossData[] bossData;
    public float hp;
    private int maxHp;
    public int dmg;

    public Vector2 direction;
    public float moveSpeed = 2f;

    bool isHit;
    bool isDead;
    bool isDamaged;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;
    WaitForSeconds knockBackWait;
    WaitForSeconds deadWait;
    WaitForSeconds invincibility;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
        knockBackWait = new WaitForSeconds(0.05f);
        deadWait = new WaitForSeconds(2f);
        invincibility = new WaitForSeconds(0.3f);
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

    public void EnemyInit(int level)
    {
        EnemyData Data = enemyData[level];

        moveSpeed = Data.moveSpeed;
        maxHp = Data.maxHp;
        hp = Data.curHp;
        spriter.sprite = Data.enemySprite;
    }

    public void BossInit(int level)
    {
        Debug.Log("���� ������ �ʱ�ȭ");
        FarmingBossData data = bossData[level];

        // �ӽ÷� ũ�� ���� ���
        transform.localScale = new Vector3(3f, 3f, 3f);

        gameObject.layer = LayerMask.NameToLayer("Boss");
        moveSpeed = data.moveSpeed;
        maxHp = data.maxHp;
        hp = data.curHp;
        spriter.sprite = data.bossSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            if (gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                Debug.Log("�������� Ŭ����");
                GameManager.Instance.isDungeonClear = true;
                
                // ���� �����۵��� �����ִ� ui ���
                GameManager.Instance.notice.DungeonClear();
            }


            gameObject.SetActive(false);
        }
        if (collision.CompareTag("Range"))
        {
            float damage = collision.GetComponent<Range>().damage;
            damage += Random.Range(-0.15f * damage, 0.15f * damage);

            // ���� ���� ��������ŭ ü�� ����
            hp -= damage;

            // PoolManager�� ������ Text�� ����
            GameObject damageText = GameManager.Instance.pool.Get(4);
            Vector3 randomPos = transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f),0); // �ؽ�Ʈ�� ������ ��ġ���� ����
            damageText.GetComponent<TextMeshProUGUI>().rectTransform.position = Camera.main.WorldToScreenPoint(randomPos); // ��ü�� ��ġ�� rectTransform���� ��ȯ
            damageText.GetComponent<DamageText>().Damaged(damage);

            if (gameObject.activeSelf)
                StartCoroutine(KnockBack());
                
            if(hp > 0)
            {
                // �ǰ� �ִϸ��̼�, ȿ���� ���
            }
            else
            {
                if(gameObject.layer == LayerMask.NameToLayer("Boss") && !GameManager.Instance.isDungeonClear)
                {
                    GameManager.Instance.isDungeonClear = true;
                    StartCoroutine(Dead());
                    DropItem();
                    // �ӽ� ����
                    Debug.Log("�������� Ŭ����");                   

                    // ���� �����۵��� �����ִ� ui ���
                    GameManager.Instance.notice.DungeonClear();
                }
                else
                {
                    StartCoroutine(Dead());
                    DropItem();

                }
                             
            }
        }
        if(collision.CompareTag("Melee") && !isDamaged)
        {
            float damage = collision.GetComponent<Melee>().damage;
            damage += Random.Range(-0.15f * damage, 0.15f * damage);

            // ���� ���� ��������ŭ ü�� ����
            hp -= damage;

            // PoolManager�� ������ Text�� ����
            GameObject damageText = GameManager.Instance.pool.Get(4);
            Vector3 randomPos = transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0); // �ؽ�Ʈ�� ������ ��ġ���� ����
            damageText.GetComponent<TextMeshProUGUI>().rectTransform.position = Camera.main.WorldToScreenPoint(randomPos); // ��ü�� ��ġ�� rectTransform���� ��ȯ
            damageText.GetComponent<DamageText>().Damaged(damage);

            if (gameObject.activeSelf)
                StartCoroutine(KnockBack());

            if (hp > 0)
            {
                // �ǰ� �ִϸ��̼�, ȿ���� ���
            }
            else
            {
                if (gameObject.layer == LayerMask.NameToLayer("Boss") && !GameManager.Instance.isDungeonClear)
                {
                    GameManager.Instance.isDungeonClear = true;
                    StartCoroutine(Dead());
                    DropItem();
                    // �ӽ� ����
                    Debug.Log("�������� Ŭ����");
                    
                    // ���� �����۵��� �����ִ� ui ���
                    GameManager.Instance.notice.DungeonClear();
                }
                else
                {
                    StartCoroutine(Dead());
                    DropItem();
                }
            }        
        }
        if (collision.CompareTag("Explosion"))
        {
            float damage = collision.GetComponent<Explosion>().damage;
            damage += Random.Range(-0.15f * damage, 0.15f * damage);

            // ���� ���� ��������ŭ ü�� ����
            hp -= damage;

            // PoolManager�� ������ Text�� ����
            GameObject damageText = GameManager.Instance.pool.Get(4);
            Vector3 randomPos = transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0); // �ؽ�Ʈ�� ������ ��ġ���� ����
            damageText.GetComponent<TextMeshProUGUI>().rectTransform.position = Camera.main.WorldToScreenPoint(randomPos); // ��ü�� ��ġ�� rectTransform���� ��ȯ
            damageText.GetComponent<DamageText>().Damaged(damage);

            if (gameObject.activeSelf)
                StartCoroutine(KnockBack());

            if (hp > 0)
            {
                // �ǰ� �ִϸ��̼�, ȿ���� ���
            }
            else
            {
                if (gameObject.layer == LayerMask.NameToLayer("Boss") && !GameManager.Instance.isDungeonClear)
                {
                    GameManager.Instance.isDungeonClear = true;
                    StartCoroutine(Dead());
                    DropItem();
                    // �ӽ� ����
                    Debug.Log("�������� Ŭ����");
                    
                    // ���� �����۵��� �����ִ� ui ���
                    GameManager.Instance.notice.DungeonClear();
                }
                else
                {
                    StartCoroutine(Dead());
                    DropItem();
                }
            }
        }
    }

    IEnumerator KnockBack()
    {       
        yield return wait;
        isHit = true;
        isDamaged = true;

        yield return knockBackWait;
        isHit = false;

        yield return invincibility;
        isDamaged = false;
    }

    public IEnumerator Dead()
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
        string[] randomPick = GameManager.Instance.ranItem.GetRandomPick();

        if (randomPick[0] != "null")
        {
            string itemName = randomPick[0];
            string grade = randomPick[1];

            GameObject ranDropItem = GameManager.Instance.pool.Get(2);
            ranDropItem.transform.position = transform.position;
            DropItem dropItem = ranDropItem.GetComponent<DropItem>();
            dropItem.ReadItemInfo(itemName);
            Sprite sprite = dropItem.GetComponent<SpriteRenderer>().sprite;

            // ���� ������ ����
            Debug.Log("������ ����");
            GameManager.Instance.CollectItem(itemName, grade, sprite);

            if (!gameObject.activeSelf)
            {
                StartCoroutine(ActivateAndNotice(itemName, grade, sprite));
            }
            else
            {
                NoticeItem(itemName, grade, sprite);
            }          
        }
        else
        {
            return;
        }
    }

    void NoticeItem(string itemName, string grade, Sprite sprite)
    {
        if(grade == "rare")
        {
            Debug.Log(grade);
            GameManager.Instance.notice.NoticeRoutine();
            GameManager.Instance.notice.noticeText.text = itemName + "�� ȹ���߽��ϴ�!";
            GameManager.Instance.notice.noticeIcon.sprite = sprite;
        }
        else
        {
            return;
        }
    }

    IEnumerator ActivateAndNotice(string itemName, string grade, Sprite sprite)
    {
        gameObject.SetActive(true);
        yield return null; 

        NoticeItem(itemName, grade, sprite);

        gameObject.SetActive(false);
    }
}
