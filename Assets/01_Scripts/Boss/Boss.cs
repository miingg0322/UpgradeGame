using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int hp;
    public int dmg;
    public float moveSpeed;
    public float atkSpeed;
    public AOE aoe;


    void Start()
    {
        //AOEAttack(5f, 90, 1f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThrowPattern()
    {

    }

    public void AOEAttack()
    {
        float radius = 5f;
        float angle = 90f;
        float activeDelay = 1f;
        float deactiveDelay = 2f;

        aoe.SetRadius(radius);
        aoe.SetAngle(angle);
        StartCoroutine(aoe.AOECoroutine(activeDelay, deactiveDelay));
    }
}
