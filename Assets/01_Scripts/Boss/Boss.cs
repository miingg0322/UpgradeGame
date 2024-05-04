using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    BossData bossData;
    public int hp;
    public int dmg;
    public float moveSpeed = 0.5f;
    public bool isMoving = false;

    public float atkSpeed = 3f;
    public float atkRange = 2f;
    public bool isAttack = false;

    public int patterns = 3;
    public AOE aoe;

    public GameObject throwWeapon;
    private float throwSpeed = 2f;

    public TrackingMissile[] missiles;


    public Player player;

    public bool isPattern = false;

    void Start()
    {
        InitBoss();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        Vector2 playerPos = player.transform.position;
        float playerDist = Vector2.Distance(playerPos, transform.position);
        if (playerDist > atkRange)
        {
            //RangePattern();

        }
        else
        {
            //NormalAttack();
        }
    }

    public void RangePattern()
    {
        if (!isPattern)
        {
            int pattern = Random.Range(0, patterns + 1);
            switch (pattern)
            {
                case 0:
                    ShootTrackMissiles(missiles.Length, 2f);
                    break;
                case 1:
                    //ThrowPattern();
                    AOEPattern();
                    break;
                default:
                    Move(2f);
                    break;
            }
        }
    }

    public void Move(float moveTime)
    {
        if (!isMoving)
            StartCoroutine(MoveCoroutine(moveTime));
    }

    IEnumerator MoveCoroutine(float moveTime)
    {
        isMoving = true;
        float moveTimer = 0;
        while(moveTimer < moveTime)
        {
            Vector2 playerPos = player.transform.position;
            float playerDist = Vector2.Distance(playerPos, transform.position);
            if (playerDist < atkRange)
                break;
            transform.Translate(playerPos * Time.deltaTime * moveSpeed);
            moveTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void NormalAttack()
    {
        if(!isAttack)
            StartCoroutine(NormalAttackCoroutine());
    }

    IEnumerator NormalAttackCoroutine()
    {
        isAttack = true;
        // 플레이어 공격 처리
        Debug.Log("Player Attack");
        yield return new WaitForSeconds(atkSpeed);
        isAttack = false;
    }

    public void ThrowPattern()
    {
        isPattern = true;
        Vector2 playerPos = player.transform.position;
        StartCoroutine(ThrowCoroutine(throwWeapon, playerPos, throwSpeed));
    }

    IEnumerator ThrowCoroutine(GameObject throwWeapon, Vector2 direction, float throwSpeed ,float removeTime = 5f)
    {        
        float timer = 0;
        throwWeapon.transform.position = transform.position;
        throwWeapon.SetActive(true);
        while(timer < removeTime)
        {
            throwWeapon.transform.Translate(direction * throwSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        throwWeapon.SetActive(false);
        throwWeapon.transform.position = transform.position;
        isPattern = false;
    }

    public void AOEPattern()
    {
        StartCoroutine(AOEAttackCorountine());        
    }

    IEnumerator AOEAttackCorountine()
    {
        float radius = 5f;
        float angle = 90f;
        float activeDelay = 1f;
        float deactiveDelay = 2f;
        isPattern = true;
        aoe.SetRadius(radius);
        aoe.SetAngle(angle);
        yield return StartCoroutine(aoe.AOECoroutine(activeDelay, deactiveDelay));
        isPattern = false;
    }

    public void ShootTrackMissiles()
    {
        int amount = 3;
        float delay = 2f;
        isPattern = true;
        if (amount > missiles.Length)
            amount = missiles.Length;
        StartCoroutine(ShootCoroutine(amount, delay));
    }
    public void ShootTrackMissiles(int amount = 3, float delay = 2f)
    {
        isPattern = true;
        if (amount > missiles.Length)
            amount = missiles.Length;
        StartCoroutine(ShootCoroutine(amount, delay));
    }

    IEnumerator ShootCoroutine(int amount, float delay)
    {
        for (int i = 0; i < amount; i++)
        {
            missiles[i].gameObject.SetActive(true);
            missiles[i].TrackTarget(player.gameObject);
            yield return new WaitForSeconds(delay);
        }
        isPattern = false;
    }

    public void InitBoss()
    {
        hp = bossData.maxHp;
        dmg = bossData.baseDmg;
        moveSpeed = bossData.baseMoveSpeed;
        atkSpeed = bossData.baseMoveSpeed;
        atkRange = bossData.atkRange;
    }
}
