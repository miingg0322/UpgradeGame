using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public int damage = 75;

    Rigidbody2D rigid;
    //SpriteRenderer spriter;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
       //spriter.sprite = Player.Instance.weapon.weaponData.sprite;
    }

    private void Update()
    {
        if (Player.Instance.playerId == 0)
            transform.Rotate(Vector3.forward * 150 * Time.deltaTime);
    }
}
