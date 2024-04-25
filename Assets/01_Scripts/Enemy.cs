using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    private int maxHp;
    public int dmg;
    public float range = 3f;
    public float moveSpeed = 2f;
    public float atkCoolTime = 2f;
    [SerializeField]
    private float atkTimer = 0f;

    public SpriteRenderer enemyRend;
    private Player player;

    public List<GameObject> drops = new List<GameObject>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        Vector2 curPos = transform.position;
        Vector2 playerPos = player.transform.position;
        float distance = Vector2.Distance(curPos, playerPos);
        if (distance > range)
        {
            transform.position = Vector2.MoveTowards(curPos, playerPos, moveSpeed * Time.deltaTime);
        }
        else
        {
            if (atkTimer <= 0)
            {
                NormalAttack();
            }
            else
            {
                atkTimer -= Time.deltaTime;
            }
        }
    }

    void NormalAttack()
    {
        Debug.Log("공격처리");
        atkTimer = atkCoolTime;
    }
}
