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

    public void UpgradeWeapon()
    {
        weapon = weaponSlot.weapon;
        if (weapon.Level<weapon.weaponData.maxLevel && weapon.Level>=0)
        {
            // ��ȭ ��� ó��
            cost = GameManager.Instance.dataTables["Cost"][weapon.weaponData.grade][weapon.Level];
            Item foundItem = SQLiteManager.Instance.inventory.FindItemExists(1, weapon.weaponData.grade);
            int amount = SQLiteManager.Instance.inventory.inventory[foundItem];
            // ��� ����
            if (foundItem !=null)
            {
                foundItem.PrintDetail();
                if (amount < cost)
                {
                    return;
                }
                SQLiteManager.Instance.inventory.inventory[foundItem] -= cost;
                SQLiteManager.Instance.UseItemFromInventory(foundItem, cost);
                SQLiteManager.Instance.inventory.GetSlotOfItem(foundItem).SetItemAmountText(SQLiteManager.Instance.inventory.inventory[foundItem]);
            }
            else
            {
                Debug.Log("��� ����");
                return;
            }
            // ��ȭ Ȯ�� ó��
            success = GameManager.Instance.dataTables["Upgrade"][weapon.weaponData.grade][weapon.Level];
            //Debug.Log(success);
            //Debug.Log(GameManager.Instance.probBase);
            int prob = Random.Range(0, GameManager.Instance.probBase);
            //Debug.Log(prob);
            Debug.Log($"��ȭ ��: {weapon.Level}");
            if (prob < success)
            {
                Debug.Log("��ȭ ����");
                UpgradeSuccess();
            }
            else
            {
                UpgradeFail();
            }
            Debug.Log($"��ȭ ��: {weapon.Level}");
            SQLiteManager.Instance.UpgradeWeapon(weaponSlot.item.name, weapon.Level);
            SQLiteManager.Instance.inventory.FindItemExists(weaponSlot.item.type, weaponSlot.item.grade).value++;
        }

    }

    private void UpgradeSuccess()
    {
        weapon.Level++;
    }
    private void UpgradeFail()
    {
        destroy = GameManager.Instance.dataTables["Destroy"][weapon.weaponData.grade][weapon.Level];
        if (destroy == 0)
            return;

        int fail = Random.Range(0, GameManager.Instance.probBase);
        if(fail < destroy)
        {
            weapon.DestroyWeapon();
        }
    }
}
