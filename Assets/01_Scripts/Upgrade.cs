using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public Weapon weapon;
    void Start()
    {
        
    }

    public void GetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
    }
    public void UpgradeWeapon()
    {
        if(weapon.Level<weapon.weaponData.maxLevel)
        {

        }
        // ��ȭ ��� ó��

        // ��ȭ Ȯ�� ó��


    }
}
