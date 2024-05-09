using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;

    private void Awake()
    {
        GameManager.Instance.AssignPool(this);
        pools = new List<GameObject>[prefabs.Length];

        for(int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

        public GameObject Get(int index)
    {
        GameObject select = null;

        foreach(GameObject pool in pools[index])
        {
            if (!pool.activeSelf)
            {
                select = pool;
                select.SetActive(true);
                break;
            }
        }

        if(select == null)
        {
            select = Instantiate(prefabs[index],transform);
            pools[index].Add(select);
        }

        return select;
    }
}
