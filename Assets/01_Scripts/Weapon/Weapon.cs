using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public WeaponBaseData weaponData;
    public int dmg;
    public Sprite sprite;
    public float atkSpeed;
    // -1 레벨 = 파괴된 상태
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
            if(level%5 == 0)
            {
                int count = level / 5;
                atkSpeed  = weaponData.atkSpeed - count * 0.1f;
            }
            Debug.Log($"{level}, {dmg}({weaponData.dmgBase} + {weaponData.dmgPerLevel * level})");
        }
    }
    public int destroyedLevel = -1;

    // 실패 없는 강화 단계 최대치
    public bool isDestroyed = false;

    public int upCost;
    public int reCost;
    public int upProb;

    void Start()
    {

    }

    public void DestroyWeapon()
    {
        // 확률 처리 후
        isDestroyed = true;
        destroyedLevel = Level;
        Level = -1;
    }

    public void SetWeaponData(int job, int grade)
    {
        Debug.Log($"WeaponData/{job}/WeaponData_{grade}");
        weaponData = Resources.Load<WeaponBaseData>($"WeaponData/{job}/WeaponData_{grade}");
    }
}


