using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    BossData bossData;
    public int hp;
    public int dmg;
    public float moveSpeed;

    public float atkSpeed = 3f;
    public float atkRange;
    public bool isAttack = false;

    public AOE aoe;
    public GameObject throwWeapon;
    private float throwSpeed = 3f;
    public Player player;

    private bool isPattern = false;

    void Start()
    {
        InitBoss();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        
    }

    public void BossPattern()
    {
        Vector2 playerPos = player.transform.position;
        float playerDist = Vector2.Distance(playerPos, transform.position);
        if (playerDist < atkRange)
            NormalAttack();
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
        yield return new WaitForSeconds(atkSpeed);
        isAttack = false;
    }

    public void ThrowPattern()
    {
        Vector2 playerPos = player.transform.position;
        StartCoroutine(ThrowCoroutine(throwWeapon, playerPos, throwSpeed));
    }

    IEnumerator ThrowCoroutine(GameObject throwWeapon, Vector2 direction, float throwSpeed ,float removeTime = 5f)
    {
        float timer = 0;
        throwWeapon.SetActive(true);
        while(timer < removeTime)
        {
            throwWeapon.transform.Translate(direction * throwSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        throwWeapon.SetActive(false);
        throwWeapon.transform.position = transform.position;
        Debug.Log(timer);
    }

    public void AOEAttack()
    {
        float radius = 5f;
        float angle = 90f;
        float activeDelay = 1f;
        float deactiveDelay = 2f;

        aoe.SetRadius(radius);
        aoe.SetAngle(angle);
        StartCoroutine(aoe.AOECoroutine(activeDelay, deactiveDelay));
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
