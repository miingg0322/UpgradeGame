using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public int damage;

    Rigidbody2D rigid;
    Transform trans;
    //SpriteRenderer spriter;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
    }

    private void Start()
    {
        damage = Player.Instance.weapon.dmg * 4;
        //spriter.sprite = Player.Instance.weapon.weaponData.sprite;                
    }

    private void OnEnable()
    {
        if (Player.Instance.playerId == 2)
        {
            trans.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            damage = 1;
            StartCoroutine(Explosion());
        }
    }

    private void Update()
    {
        if (Player.Instance.playerId == 0)
            transform.Rotate(Vector3.forward * 150 * Time.deltaTime);      
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(0.85f);

        GameObject explosion = GameManager.Instance.pool.Get(5);
        explosion.transform.parent = transform.parent;
        explosion.transform.position = transform.position;

        yield return new WaitForSeconds(0.15f);

        gameObject.SetActive(false);
    }
}
