using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    public int bossSpawnCount = 30;
    public int renewalSpecialMove = 20;

    float timer;
    int spawnDataIndex;
    int count;
    bool isBossSpawn;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Start()
    {
        spawnDataIndex = GameManager.Instance.DungeonLevel;
        count = 0;
        Debug.Log("count 초기화");
    }
    void Update()
    {
        if (GameManager.Instance.isDungeonClear)
            return;

        timer += Time.deltaTime;

        if (timer > spawnData[spawnDataIndex].spawnTime)
        {
            timer = 0;
            Spawn();
            count++;

            if (count == bossSpawnCount)
                isBossSpawn = true;
        }
        if(count == renewalSpecialMove)
        {
            Player.Instance.specialMove = true;
            GameManager.Instance.notice.ActiveSpecialMove();
        }

        if (isBossSpawn)
        {
            Debug.Log("보스 소환");
            SpawnBoss();
            isBossSpawn = false;
        }
            
    }

    void Spawn()
    {
        GameObject enemy = GameManager.Instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(0, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().EnemyInit(spawnDataIndex); 
    }

    void SpawnBoss()
    {
        GameObject boss = GameManager.Instance.pool.Get(6);
        boss.transform.position = spawnPoint[9].position;
        boss.GetComponent<Enemy>().BossInit(spawnDataIndex);        
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
}
