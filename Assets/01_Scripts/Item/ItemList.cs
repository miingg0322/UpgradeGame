using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    public int maxType;
    public int maxGrade;

    public List<ItemData> allItemDataList = new List<ItemData>();
    public List<List<ItemData>> itemListByType = new List<List<ItemData>>();
    public List<List<ItemData>> itemListByGrade = new List<List<ItemData>>();

    public List<List<ItemData>> itemDataList = new List<List<ItemData>>();
    public List<ItemData> weaponList = new List<ItemData>();
    public List<ItemData> stoneList = new List<ItemData>();
    public List<ItemData> repairList = new List<ItemData>();
    public List<ItemData> forgeList = new List<ItemData>();

    public List<List<Sprite>> spriteLists = new List<List<Sprite>>();
    public List<Sprite> weaponSprites = new List<Sprite>();
    public List<Sprite> stoneSprites = new List<Sprite>();
    public List<Sprite> repairSprites = new List<Sprite>();
    public List<Sprite> forgeSprites = new List<Sprite>();

    private void Start()
    {
        spriteLists.Add(weaponSprites);
        spriteLists.Add(stoneSprites);
        spriteLists.Add(repairSprites);
        spriteLists.Add(forgeSprites);
    }
    public void SortItem(string[][] itemList)
    {
        foreach (var data in itemList)
        {
            ItemData item = new ItemData(data);
            allItemDataList.Add(item);
            switch (item.type)
            {
                case 0:
                    weaponList.Add(item);
                    break;
                case 1:
                    stoneList.Add(item);
                    break;
                case 2:
                    repairList.Add(item);
                    break;
                case 3:
                    forgeList.Add(item);
                    break;
                default:
                    break;
            }
        }
    }

    public ItemData GetItemData(int type, int grade)
    {
        Debug.Log($"{type}, {grade}");
        Debug.Log(itemListByType.Count);
        Debug.Log(itemListByType[type].Count);
        ItemData result = itemListByType[type][grade];
        Debug.Log(result.name);
        return result;
    }

    public Sprite GetItemSprite(int type, int grade)
    {
        return spriteLists[type][grade];
    }

    public void InitLists()
    {
        for (int i = 0; i <= maxType; i++)
        {
            List<ItemData> typeList = new List<ItemData>();
            itemListByType.Add(typeList);
        }
        for (int i = 0; i <= maxGrade; i++)
        {
            List<ItemData> gradeList = new List<ItemData>();
            itemListByGrade.Add(gradeList);
        }
        foreach (var itemData in allItemDataList)
        {
            int type = itemData.type;
            int grade = itemData.grade;
            //Debug.Log($"Type:{type}, Grade:{grade}");
            itemListByType[type].Add(itemData);
            itemListByGrade[grade].Add(itemData);
        }
        Debug.Log($"{itemListByType[0].Count}");
    }
}

public class ItemData
{
    public string name;
    public int type;
    public int grade;
    public string desc; 
    public int weight;
    public string enName;
    public string enDesc;

    public ItemData() { }
    public ItemData(string[] data)
    {
        name = data[0];
        type = int.Parse(data[1]);
        grade = int.Parse(data[2]);
        desc = data[3];
        weight = int.Parse(data[4]);
        enName = data[5];
        enDesc = data[6];
    }
}
public class Item : ItemData
{
    public int equipped = -1;
    public int value;

    public Item(ItemData data, int value = -1, int equipped = -1)
    {
        name = data.name;
        type = data.type;
        grade = data.grade;
        desc = data.desc;
        weight = data.weight;
        enName = data.enName;
        enDesc = data.enDesc;
        this.value = value;
        this.equipped = equipped;
    }
    public Item(string name, int type, int grade, int value = -1, int equipped = -1)
    {
        this.name = name;
        this.type = type;
        this.grade = grade;
        this.value = value;
    }
    public void PrintDetail()
    {
        Debug.Log($"{name}, {type}, {grade}, {value}");
    }
}
