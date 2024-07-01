using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    bool isNear;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isNear)
        {
            Debug.Log("·©Å· UI Ãâ·Â");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNear = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNear = false;
        }
    }
}
