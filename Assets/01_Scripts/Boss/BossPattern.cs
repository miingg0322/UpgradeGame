using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern
{
    public string pName;
    public bool isCoolDown = false;
    public float coolDown;
    public float coolDownTimer;
    public int priority;
    public delegate void Pattern();
    public Pattern pattern;

    public BossPattern(string pName, float coolDown, int priority)
    {
        this.pName = pName;
        isCoolDown = false;
        this.coolDown = coolDown;
        coolDownTimer = coolDown;
        this.priority = priority;
    }

    public void UpdateCoolDown()
    {
        if (isCoolDown)
        {
            coolDownTimer -= Time.deltaTime;
            //Debug.Log(coolDownTimer);
            if (coolDownTimer <= 0)
            {
                coolDownTimer = coolDown;
                isCoolDown = false;
                Debug.Log($"{pName} 사용 가능");
            }
        }
    }

    public void DoPattern(Boss boss, bool isCoolPattern = true)
    {
            pattern();
            if (isCoolPattern)
            {
                isCoolDown = true;
            }

    }
}
