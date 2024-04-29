using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RanItemData", menuName = "Scriptable Objects/RanItemData", order = 1)]
public class RanItemData : ScriptableObject
{
    [System.Serializable]
    public struct ItemInfo
    {
        public string itemName;
        public double weight;
    }

    public ItemInfo[] itemInfos;
}
