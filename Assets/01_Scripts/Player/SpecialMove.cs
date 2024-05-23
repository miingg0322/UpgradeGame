using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialMove : MonoBehaviour
{
    public int damage = 300;
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

        if (Player.Instance.playerClass == 2)
        {
            spriter.color = Color.white;
            transform.localScale = Vector3.one;
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
        transform.Translate(Vector2.left * 30 * Time.fixedDeltaTime);
    }
    void BerserkerSpecialMove()
    {       
        transform.Rotate(0, 0, 30);
        rigid.velocity = Vector2.left * 20;

        if(timer > 0.3f)
        {
            damage = 40;
            rigid.velocity = Vector2.zero;
        }
        if (timer < 3f)
        {                     
            timer += Time.fixedDeltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }
    IEnumerator GunslingerSpecialMove()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = Vector3.zero;

        while (timer < 100)
        {
            float t = timer / 100;
            // 포물선 공식 사용: y = h * 4 * t * (1 - t)
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, t);
            currentPosition.y = 8 * t * (1 - t); // 시작과 끝 위치의 y 좌표를 고려하여 계산
            gameObject.transform.position = currentPosition;

            timer += Time.fixedDeltaTime;
            yield return null;
        }

        gameObject.transform.position = targetPosition;

        if (timer > 100)
        {
            yield return new WaitForSeconds(1);
            explosion = true;           
        }
        if (explosion)
        {
            spriter.color = Color.red;
            transform.localScale += Vector3.one * 15 * Time.fixedDeltaTime;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall"))
            gameObject.SetActive(false);
    }
}
