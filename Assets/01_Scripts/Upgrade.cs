using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public Weapon weapon;
    public int success;
    public int destroy;

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
            // 강화 비용 처리

            // 강화 확률 처리
            success = GameManager.Instance.dataTables["Upgrade"][weapon.weaponData.tier][weapon.Level];
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
