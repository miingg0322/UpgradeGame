using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public PlayerData[] playerDatas;
    public float maxHealth;
    public float curHealth;
    public float speed;
    public float drainRate;

    public Vector2 inputVec;
    public Scanner scanner;
    public Weapon weapon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }


    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    public void Init(int playerId)
    {
        PlayerData data = playerDatas[playerId];

        maxHealth = data.maxHp;
        curHealth = data.curHp;
        speed = data.moveSpeed;
        drainRate = data.drainRate;
        spriter.sprite = data.playerSprite;
    }
}
