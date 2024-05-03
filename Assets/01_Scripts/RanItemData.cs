using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

[CreateAssetMenu(fileName = "RanItemData", menuName = "Scriptable Objects/RanItemData", order = 1)]
public class RanItemData : ScriptableObject
{
    [System.Serializable]
    public struct ItemInfo
    {
        public string itemName;
        public double weight;
        public string grade;
    }

    public ItemInfo[] itemInfos;

    public string GetGrade(string itemName)
    {
        foreach(var itemInfo in itemInfos)
        {
            if(itemInfo.itemName == itemName)
            {
                return itemInfo.grade;
            }
        }
        return "normal";
    }
}
