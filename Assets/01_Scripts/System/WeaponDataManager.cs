using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDataManager : MonoBehaviour
{
    public WeaponBaseData[] warriorWeaponDatas;
    public WeaponBaseData[] berserkerWeaponDatas;
    public WeaponBaseData[] gunslingerWeaponDatas;

    // ������ ��޿� �ش��ϴ� �����͸� ��ȯ�ϴ� �Լ�
    public WeaponBaseData GetWeaponData(int classId, int grade)
    {
        WeaponBaseData[] jobWeaponDatas = GetJobWeaponDatas(classId);
        if (jobWeaponDatas == null)
        {
            Debug.LogError("�߸��� ���� �Է�");
            return null;
        }

        if (grade < 0 || grade >= jobWeaponDatas.Length)
        {
            Debug.LogError("index ����");
            return null;
        }

        return jobWeaponDatas[grade];
    }

    // ������ �ش��ϴ� ������ �迭�� ��ȯ�ϴ� �Լ�
    private WeaponBaseData[] GetJobWeaponDatas(int classId)
    {
        switch (classId)
        {
            case 0:
                return warriorWeaponDatas;
            case 1:
                return berserkerWeaponDatas;
            case 2:
                return gunslingerWeaponDatas;
            default:
                return null;
        }
    }
}
