using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    float timer;
    int spawnDataIndex;

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
        timer += Time.deltaTime;

        if (timer > spawnData[spawnDataIndex].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.Instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(0, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnDataIndex); 
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
}
