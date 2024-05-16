using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    [SerializeField]
    internal BossData bossData;
    public int hp;

    public float moveTime = 2f;
    public bool isMoving = false;
    public bool isAttack = false;
    public bool isPattern = false;

    public Player player;

    public bool isPatternCoolDown = false;
    internal float patternCoolDown = 10f;
    [SerializeField]
    internal float patternCoolTimer = 0;

    public List<BossPattern> bossPatterns = new List<BossPattern>();
    void Start()
    {
        InitBoss();
    }

    public virtual void InitBoss()
    {
        hp = bossData.maxHp;
        patternCoolTimer = patternCoolDown;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public virtual void MoveToPlayer()
    {
        if(!isMoving)
            StartCoroutine(MoveToPlayerCo(moveTime));
    }

    public virtual void MoveToPlayer(float moveTime)
    {
        if (!isMoving)
            StartCoroutine(MoveToPlayerCo(moveTime));
    }

    internal virtual IEnumerator MoveToPlayerCo(float moveTime)
    {
        isMoving = true;
        float moveTimer = 0;
        while (moveTimer < moveTime)
        {
            Vector2 playerPos = player.transform.position;
            Vector2 direction = playerPos - (Vector2)transform.position;
            direction = direction.normalized;
            float playerDist = Vector2.Distance(playerPos, transform.position);
            if (playerDist < bossData.atkRange)
                break;
            transform.Translate(direction * Time.deltaTime * bossData.moveSpeed);
            moveTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isMoving = false;
    }

    public virtual void BossAttack()
    {
        if (!isAttack)
            StartCoroutine(BossAttackCoroutine());
    }

    internal virtual IEnumerator BossAttackCoroutine()
    {
        isAttack = true;
        // 플레이어 공격 처리
        Debug.Log("Player Attack");
        yield return new WaitForSeconds(bossData.atkSpeed);
        isAttack = false;
    }

    public virtual bool IsPlayerInAtkRange()
    {
        Vector2 playerPos = player.transform.position;
        float playerDist = Vector2.Distance(playerPos, transform.position);
        if (playerDist <= bossData.atkRange)
            return true;
        else
            return false;
    }

    internal IEnumerator StopWhilePatternCo(float stopTime = 2f)
    {
        isPattern = true;
        yield return new WaitForSeconds(stopTime);
        isPattern = false;
    }

    public virtual void PatternCoolTimeCountDown()
    {
        if (isPatternCoolDown)
        {
            patternCoolTimer -= Time.deltaTime;
            if (patternCoolTimer <= 0)
            {
                isPatternCoolDown = false;
                patternCoolTimer = patternCoolDown;
            }
        }
    }

    public virtual void DoPriorityPattern(List<BossPattern> bossPatterns)
    {
        int highest = 0;
        BossPattern priorityPattern = null;
        isPatternCoolDown = true;
        foreach (var pattern in bossPatterns)
        {
            if(!pattern.isCoolDown && pattern.priority > highest)
            {
                priorityPattern = pattern;
            }
        }
        priorityPattern?.DoPattern(this);
        Debug.Log(priorityPattern.pName);
    }
}
