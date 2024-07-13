using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedRandom : MonoBehaviour
{
    public List<ItemData> itemPool = new List<ItemData>();
    public Dictionary<ItemData, float> weightedItemDict = new Dictionary<ItemData, float>();

    void Start()
    {
        
    }

    public void SetRandom()
    {
        itemPool.Clear();
        weightedItemDict.Clear();
        //int grade = GameManager.Instance.DungeonLevel;
        int grade = 0;
        itemPool = SheetManager.Instance.itemList.itemListByGrade[grade];
        int totalWeight = 0;
        itemPool.Sort((ItemData dat1, ItemData dat2)=> dat1.weight.CompareTo(dat2.weight));
        foreach (var item in itemPool)
        {
            if (item.weight > 0)
                totalWeight += item.weight;
            Debug.Log(item.weight);
        }
        Debug.Log(totalWeight);
        foreach (var item in itemPool)
        {
            float weight = (float)item.weight / totalWeight;
            if (weight > 0)
                weightedItemDict.Add(item, weight);
        }

        foreach (var item in weightedItemDict)
        {
            Debug.Log($"{item.Key.name}:::{item.Key.weight}:::{item.Value}");
        }
        
    }

    public ItemData RandomPick()
    {
        float pivot = Random.Range(0, 1f);
        float sum = 0;
        foreach (var pair in weightedItemDict)
        {
            sum += pair.Value;
            if(sum >= pivot)
            {
                return pair.Key;
            }
        }
        return null;
    }

    public void RandomPickTest(int num)
    {
        Dictionary<ItemData, int> countDict = new Dictionary<ItemData, int>();
        foreach (var item in weightedItemDict)
        {
            countDict.Add(item.Key, 0);
        }
        for (int i = 0; i < num; i++)
        {
            ItemData picked = RandomPick();
            countDict[picked]++;
        }

        foreach (var item in countDict)
        {
            Debug.Log($"{item.Key.name}:::»ÌÈù È½¼ö: {item.Value}/ È®·ü: {(float)item.Value / num}");
        }

    }
}
