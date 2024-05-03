using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "ScriptableObjects/BossData", order = 2)]
public class BossData : ScriptableObject
{
    public int maxHp;
    public int baseDmg;
    public float baseMoveSpeed;
    public float atkRange;
}
