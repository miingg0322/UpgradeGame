using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponBaseData", menuName = "ScriptableObjects/WeaponBaseData", order = 1)]
public class WeaponBaseData : ScriptableObject
{
    public int tier;
    public int dmgBase;
    public int dmgPerLevel;
    public int maxSafeLevel = 20;
}
