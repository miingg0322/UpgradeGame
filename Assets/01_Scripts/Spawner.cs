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
        enemy.GetComponent<Enemy>().Init(spawnData[0]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public float speed;
    public int level;
    public int health;
    public int spriteType;
}
