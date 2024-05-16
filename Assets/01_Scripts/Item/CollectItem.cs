using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollectItem
{
    public string itemName;
    public string itemGrade;
    public Sprite itemImage;
    public int itemQuantity;

    public CollectItem(string name, string grade, Sprite image, int qty)
    {
        itemName = name;
        itemGrade = grade;
        itemImage = image;
        itemQuantity = qty;
    }
}
