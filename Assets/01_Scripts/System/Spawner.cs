using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    public int maxSpawnCount = 30;

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

            if (count == maxSpawnCount)
                isBossSpawn = true;
        }

        if (isBossSpawn)
        {
            Debug.Log("보스 소환");
            SpawnBoss();
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
        GameObject boss = GameManager.Instance.pool.Get(0);
        boss.transform.position = spawnPoint[8].position;
        boss.GetComponent<Enemy>().BossInit(spawnDataIndex);
        isBossSpawn = false;
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
}
