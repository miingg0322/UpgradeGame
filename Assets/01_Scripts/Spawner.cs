using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    void Update()
    {
        timer += Time.deltaTime;

        // 플레이어가 입장한 던전의 level 값을 받아와서 spawnData[]에 값을 할당하도록 수정 예정
        if (timer > spawnData[0].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.Instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(0, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[0].level); // 플레이어가 입장한 던전의 level 값을 받아와서 spawnData[]에 값을 할당하도록 수정 예정
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int level;
}
