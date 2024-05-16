using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBoss : Boss
{
    public GameObject throwWeapon;
    public float throwSpeed = 1f;

    public TrackingMissile[] missiles;

    public AOE aoe;

    public List<BossPattern> rangePatterns = new List<BossPattern>();
    public List<BossPattern> aroundPatterns = new List<BossPattern>();

    private void Start()
    {
        InitBoss();
        aoe = transform.Find("AOE").GetComponent<AOE>();
        BossPattern bossMove = new BossPattern("Move", 0, 1);
        bossMove.pattern = MoveToPlayer;
        BossPattern bossAttack = new BossPattern("Attack", bossData.atkSpeed, 1);
        BossPattern bossThrow = new BossPattern("Throw", 20f, 2);
        bossThrow.pattern = ThrowPattern;
        BossPattern bossTracker = new BossPattern("Tracker", 30f, 3);
        bossTracker.pattern = ShootTrackMissiles;
        BossPattern bossAOE = new BossPattern("AOE", 10f, 3);
        bossAOE.pattern = AOEPattern;

        bossPatterns.AddRange(new List<BossPattern>{ bossThrow, bossTracker, bossAOE});
        rangePatterns.AddRange(new List<BossPattern> { bossThrow, bossTracker });
        aroundPatterns.AddRange(new List<BossPattern> { bossAOE, bossThrow });
        //MoveToPlayer(1f);
    }
    void Update()
    {

        PatternCoolTimeCountDown();

        foreach (var bossPattern in bossPatterns)
        {
            bossPattern.UpdateCoolDown();
        }


        if (IsPlayerInAtkRange())
        {
            if (isPatternCoolDown)
            {
                if (!isPattern)
                    BossAttack();
            }
            else
            {
                DoPriorityPattern(aroundPatterns);
            }
        }
        else
        {
            if (isPatternCoolDown)
            {
                if(!isPattern)
                    MoveToPlayer();
            }
            else
            {
                DoPriorityPattern(rangePatterns);
            }
        }
       


    }


    public void ThrowPattern()
    {
        Vector2 playerPos = player.transform.position;
        StartCoroutine(StopWhilePatternCo());
        StartCoroutine(ThrowCoroutine(throwWeapon, playerPos, throwSpeed));
    }

    IEnumerator ThrowCoroutine(GameObject throwWeapon, Vector2 direction, float throwSpeed, float removeTime = 5f)
    {
        float timer = 0;
        throwWeapon.transform.position = transform.position;
        throwWeapon.SetActive(true);

        while (timer < removeTime)
        {
            throwWeapon.transform.Translate(direction * throwSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        throwWeapon.SetActive(false);
        throwWeapon.transform.position = transform.position;
    }


    public void AOEPattern()
    {
        StartCoroutine(AOEAttackCorountine());
    }

    IEnumerator AOEAttackCorountine()
    {       
        StartCoroutine(StopWhilePatternCo(aoe.activeDelay + aoe.deactiveDelay + 1f));
        yield return StartCoroutine(aoe.AOECoroutine());
    }

    public void ShootTrackMissiles()
    {
        int amount = 3;
        float delay = 1f;
        if (amount > missiles.Length)
            amount = missiles.Length;
        StartCoroutine(StopWhilePatternCo(delay * amount + 1f));
        StartCoroutine(ShootCoroutine(amount, delay));
    }
    public void ShootTrackMissiles(int amount = 3, float delay = 2f)
    {
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
    }
}
