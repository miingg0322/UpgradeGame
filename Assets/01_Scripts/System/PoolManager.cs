using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] objPools;

    private void Awake()
    {
        GameManager.Instance.AssignPool(this);
        objPools = new List<GameObject>[prefabs.Length];

        for(int index = 0; index < objPools.Length; index++)
        {
            objPools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach(GameObject pool in objPools[index])
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
            select = Instantiate(prefabs[index], transform);

            // 4번 pool은 Text라 Canvas에 생성되도록 설정
            if (index == 4)
            {
                GameObject canvasObject = GameObject.Find("Canvas");
                Canvas canvas = canvasObject.GetComponent<Canvas>();

                select.transform.SetParent(canvas.transform, false);
            }
                

            objPools[index].Add(select);
        }

        return select;
    }
}
