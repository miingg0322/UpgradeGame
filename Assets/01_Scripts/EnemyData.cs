using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptble Object/EnemyData")]
public class EnemyData : ScriptableObject
{
    public enum EnemyType { Level0, Level1, Level2, Level3, Level4 }

    [Header("# Main Info")]
    public EnemyType enemyType;
    public int enemyId;
    public string enemyName;

    [Header("# Enemy Data")]
    public int maxHp;
    public int curHp;
    public float moveSpeed;
    public Sprite enemySprite;
}
