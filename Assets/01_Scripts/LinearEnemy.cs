using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearEnemy : MonoBehaviour
{
    public Vector2 direction;
    public float moveSpeed = 2f;
    void Start()
    {
        direction = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 curPos = transform.position;
        transform.Translate(direction * moveSpeed* Time.deltaTime);
    }
}
