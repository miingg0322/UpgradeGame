using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Scriptble Object/PlayerData")]
public class PlayerData : ScriptableObject
{
    public enum PlayerType { Warrior, Berserker, Gunslinger }

    [Header("# Main Info")]
    public PlayerType playerType;
    public int playerId;
    public string playerName;
    public string playerTrait;

    [Header("# Player Data")]
    public float maxHp;
    public float curHp;
    public float moveSpeed;
    public float attackSpeed;
    public float drainRate;
    public Sprite playerSprite;
}
