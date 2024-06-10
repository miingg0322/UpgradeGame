using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDataManager : MonoBehaviour
{
    public WeaponBaseData[] warriorWeaponDatas;
    public WeaponBaseData[] berserkerWeaponDatas;
    public WeaponBaseData[] gunslingerWeaponDatas;

    // 직업과 등급에 해당하는 데이터를 반환하는 함수
    public WeaponBaseData GetWeaponData(int classId, int grade)
    {
        WeaponBaseData[] jobWeaponDatas = GetJobWeaponDatas(classId);
        if (jobWeaponDatas == null)
        {
            Debug.LogError("잘못된 직업 입력");
            return null;
        }

        if (grade < 0 || grade >= jobWeaponDatas.Length)
        {
            Debug.LogError("index 에러");
            return null;
        }

        return jobWeaponDatas[grade];
    }

    // 직업에 해당하는 데이터 배열을 반환하는 함수
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
