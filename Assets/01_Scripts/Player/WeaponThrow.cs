using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponThrow : MonoBehaviour
{
    public float speed;
    public float baseSpeed = 2f;
    public Player player;

    float timer;
    void Start()
    {
        StartCoroutine(SpeedInit());
    }
    private void Update()
    {
        if (GameManager.Instance.isDungeonClear)
            return;

        timer += Time.deltaTime;

        if (timer > speed)
        {
            timer = 0;
            RangeAttack();
        }
    }
    void RangeAttack()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - player.transform.position;
        dir = dir.normalized;

        if (Player.Instance.playerId == 2)
            dir *= 2;

        Transform weapon = GameManager.Instance.pool.Get(1).transform;
        weapon.position = player.transform.position;
        weapon.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        int damage = 300; // 플레이어가 장착한 무기의 dmg를 받아오게 수정 예정
        weapon.GetComponent<Range>().Init(dir, damage);
    }

    IEnumerator SpeedInit()
    {
        yield return null;

        speed = baseSpeed / player.attackSpeed;
    }
}
