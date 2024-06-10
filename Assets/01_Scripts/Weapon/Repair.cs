using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : MonoBehaviour
{
    public WeaponSlot weaponSlot;
    public int lowerAmount;
    public Weapon weapon;
    public Weapon weapon1;
    void Start()
    {
        //weapon.weaponData.dmgBase = 10;
    }

    public void NormalRepair()
    {
        if (weapon.isDestroyed)
        {
            weapon.Level = weapon.destroyedLevel - lowerAmount;
            weapon.destroyedLevel = -1;
            weapon.isDestroyed = false;
        }
    }

    public void RandomRepair()
    {
        if (weapon.isDestroyed)
        {
            int minLevel = weapon.destroyedLevel - lowerAmount * 2;
            int maxLevel = weapon.destroyedLevel + 1;

            int randomLevel = Random.Range(minLevel, maxLevel);
            weapon.Level = randomLevel;
            weapon.destroyedLevel = -1;
            weapon.isDestroyed = false;
        }
    }
}
