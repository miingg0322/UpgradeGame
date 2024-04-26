using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public int tier;
    public int dmg;
    public int dmgBase;
    public int dmgPerLevel;
    // -1 ���� = �ı��� ����
    private int level;
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            if (level >= 0)
                dmg = dmgBase + dmgPerLevel * level;
            else
                dmg = (int)(dmgBase * 0.75f);
            Debug.Log($"{level}, {dmg}({dmgBase} + {dmgPerLevel * level})");
        }
    }
    public int destroyedLevel = -1;

    // ���� ���� ��ȭ �ܰ� �ִ�ġ
    public int maxSafeLevel = 20;
    public bool isDestroyed = false;

    public int upCost;
    public int reCost;
    public int upProb;

    void Start()
    {
        Level = 20;

    }

    public void DestroyWeapon()
    {
        // Ȯ�� ó�� ��
        isDestroyed = true;
        destroyedLevel = Level;
        Level = -1;
    }
}


