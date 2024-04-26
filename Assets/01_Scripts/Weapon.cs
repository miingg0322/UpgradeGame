using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    [SerializeField]
    private WeaponBaseData weaponData;
    public int dmg;
    // -1 ���� = �ı��� ����
    private int level;
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            if (level >= 0)
                dmg = weaponData.dmgBase + weaponData.dmgPerLevel * level;
            else
                dmg = (int)(weaponData.dmgBase * 0.75f);
            Debug.Log($"{level}, {dmg}({weaponData.dmgBase} + {weaponData.dmgPerLevel * level})");
        }
    }
    public int destroyedLevel = -1;

    // ���� ���� ��ȭ �ܰ� �ִ�ġ
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


