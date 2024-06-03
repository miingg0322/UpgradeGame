using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public WeaponSlot weaponSlot;
    public Weapon weapon;
    public int success;
    public int destroy;
    public int cost;

    void Start()
    {
    }

    public void GetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
    }
    public void UpgradeWeapon()
    {
        if(weapon.Level<weapon.weaponData.maxLevel && weapon.Level>=0)
        {
            // ��ȭ ��� ó��
            cost = GameManager.Instance.dataTables["Cost"][weapon.weaponData.grade][weapon.Level];
            Item foundItem = SQLiteManager.Instance.inventory.FindItemExists(1, weapon.weaponData.grade);
            // ��� ����
            if (foundItem !=null)
            {
                if (foundItem.amount < cost)
                    return;
            }
            else
            {
                foundItem.amount -= cost;
                SQLiteManager.Instance.UseItemFromInventory(foundItem, cost);
            }
            // ��ȭ Ȯ�� ó��
            success = GameManager.Instance.dataTables["Upgrade"][weapon.weaponData.grade][weapon.Level];
            Debug.Log(success);
            //Debug.Log(GameManager.Instance.probBase);
            int prob = Random.Range(0, GameManager.Instance.probBase);
            //Debug.Log(prob);
            if (prob < success)
            {
                UpgradeSuccess();
            }
            else
            {
                UpgradeFail();
            }
        }

    }

    private void UpgradeSuccess()
    {
        weapon.Level++;
    }
    private void UpgradeFail()
    {
        destroy = GameManager.Instance.dataTables["Destroy"][weapon.weaponData.tier][weapon.Level];
        if (destroy == 0)
            return;

        int fail = Random.Range(0, GameManager.Instance.probBase);
        if(fail < destroy)
        {
            weapon.DestroyWeapon();
        }
    }
}
