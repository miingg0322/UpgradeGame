using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Range : MonoBehaviour
{
    public int damage;
    public float maxDistance;
    public float rotationSpeed;

    int playerClass;
    bool isTurn = false;

    Rigidbody2D rigid;
    //SpriteRenderer spriter;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        //spriter.sprite = Player.Instance.weapon.weaponData.sprite;
        playerClass = Player.Instance.playerId;
    }

    private void OnEnable()
    {
        isTurn = false;
    }
    private void FixedUpdate()
    {
        if (playerClass != 1)
        {
            return;
        }
            
        float distance = Vector2.Distance(GameManager.Instance.player.transform.position, rigid.transform.position);
        transform.Rotate(0,0, rotationSpeed);

        if(maxDistance < distance && !isTurn)
        {
            rigid.velocity *= -1f;
            isTurn = true;
        }
    }

    public void Init(Vector3 dir, int dmg)
    {
        this.damage = dmg;
        rigid.velocity = dir * 15f;
        isTurn = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && isTurn)
        {
            gameObject.SetActive(false);
        }
        if (collision.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
    }
}
