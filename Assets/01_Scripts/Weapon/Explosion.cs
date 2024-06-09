using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage;

    float timer = 0f;
    private void OnEnable()
    {
        damage = Player.Instance.weapon.dmg * 8;
        transform.localScale = Vector3.zero;
        timer = 0f;
    }
    void Update()
    {
        if (timer < 0.15f)
        {
            timer += Time.deltaTime;
            transform.localScale += Vector3.one * 10f * Time.deltaTime;
        }
        else
        {
            timer = 0;           
            gameObject.SetActive(false);          
        }        
    }
}
