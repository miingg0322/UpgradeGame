using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCleaner : MonoBehaviour
{
    float damage = 9999999;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().hp -= damage;
            StartCoroutine(collision.GetComponent<Enemy>().Dead());
        }
        if (collision.CompareTag("Range"))
        {
            Destroy(collision.gameObject);
        }
    }
}
