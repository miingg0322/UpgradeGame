using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss", menuName = "Scriptble Object/FarmingBossData")]
public class FarmingBossData : ScriptableObject
{
    public enum EnemyType { Level0, Level1, Level2, Level3, Level4 }

    [Header("# Main Info")]
    public EnemyType bossType;
    public int bossId;
    public string bossName;

    [Header("# Boss Data")]
    public int maxHp;
    public int curHp;
    public float moveSpeed;
    public Sprite bossSprite;
}
