using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialMove : MonoBehaviour
{
    public float wariorDamage = 500;
    public float berserkerDamage = 250;
    public float gunslingerDamage = 300;

    public float damage;
    float spinSpeed = 30;
    float spinDamage = 40;
    float wariorMoveSpeed = 60;
    float berserkerMoveSpeed = 20;
    float timer = 0;

    
    bool explosion;

    SpriteRenderer spriter;
    Rigidbody2D rigid;

    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        transform.position = Player.Instance.transform.position;
        timer = 0;

        switch (Player.Instance.playerClass)
        {
            case 0:
                damage = wariorDamage;
                break;
            case 1:
                damage = berserkerDamage;
                break;
            case 2:
                damage = gunslingerDamage;
                spriter.color = Color.white;
                transform.localScale = Vector3.one;
                break;
        }
    }
    private void Update()
    {
        if (gameObject.activeSelf)
        {
            timer += Time.deltaTime;

            if (timer > 0.5f && Player.Instance.playerClass == 1)
            {
                damage = spinDamage;
            }
        }
    }
    void FixedUpdate()
    {
        switch (Player.Instance.playerClass)
        {
            case 0:
                WarriorSpecialMove();
                break;
            case 1:
                BerserkerSpecialMove();
                break;
            case 2:
                StartCoroutine(GunslingerSpecialMove());
                break;
        }
    }
    void WarriorSpecialMove()
    {
        transform.Translate(Vector2.left * wariorMoveSpeed * Time.fixedDeltaTime);
    }
    void BerserkerSpecialMove()
    {       
        transform.Rotate(0, 0, spinSpeed);
        rigid.velocity = Vector2.left * berserkerMoveSpeed;

        if(timer > 0.6f)
        {          
            rigid.velocity = Vector2.zero;
        }
        if (timer > 3f)
        {
            gameObject.SetActive(false);
        }        
    }
    IEnumerator GunslingerSpecialMove()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = Vector3.zero;

        while (timer < 0.7f)
        {
            float t = timer / 0.7f;
            // 포물선 공식 사용: y = h * 4 * t * (1 - t)
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, t);
            currentPosition.y = 8 * t * (1 - t); // 시작과 끝 위치의 y 좌표를 고려하여 계산
            gameObject.transform.position = currentPosition;

            yield return null;
        }

        gameObject.transform.position = targetPosition;

        if (timer > 0.7f)
        {
            yield return new WaitForSeconds(0.3f);
            explosion = true;           
        }
        if (explosion)
        {
            spriter.color = Color.red;
            transform.localScale += Vector3.one * 20 * Time.fixedDeltaTime;
        }
        
    }   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall"))
            gameObject.SetActive(false);
    }
}
