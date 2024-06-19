using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    public List<ItemData> weaponList = new List<ItemData>();
    public List<ItemData> stoneList = new List<ItemData>();
    public List<ItemData> repairList = new List<ItemData>();
    public List<ItemData> forgeList = new List<ItemData>();
    
    public void SortItem(string[][] itemList)
    {
        foreach (var data in itemList)
        {
            ItemData item = new ItemData(data);
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
        Debug.Log($"{weaponList.Count}, {stoneList.Count}, {repairList.Count}, {forgeList.Count} ");
    }

    public ItemData GetItemData(int type, int grade)
    {
        ItemData result;
        switch (type)
        {
            case 0:
                result = weaponList.Find(item => item.grade == grade);
                break;
            case 1:
                result = stoneList.Find(item => item.grade == grade);
                break;
            case 2:
                result = repairList.Find(item => item.grade == grade);
                break;
            case 3:
                result = forgeList.Find(item => item.grade == grade);
                break;
            default:
                result = null;
                break;
        }
        return result;
    }
}

public class ItemData
{
    public string name;
    public int type;
    public int grade;
    public string desc;
    public string enName;
    public string enDesc;

    public ItemData() { }
    public ItemData(string[] data)
    {
        name = data[0];
        type = int.Parse(data[1]);
        grade = int.Parse(data[2]);
        desc = data[3];
        enName = data[4];
        enDesc = data[5];
    }
}
public class Item : ItemData
{
    public int equipped = -1;
    public int value;

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
