using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    GridLayoutGroup gridGroup;
    [SerializeField]
    private int cols;
    public InvenSlot[] invenSlots;
    void Start()
    {
        gridGroup = GetComponent<GridLayoutGroup>();
        gridGroup.constraintCount = cols;
        invenSlots = transform.GetComponentsInChildren<InvenSlot>();

        inventory = SQLiteManager.Instance.GetAllItems();
        foreach (var item in inventory)
        {
            Debug.Log($"{item.name}::{item.type}:::{item.grade} - {item.amount}");
        }
        SetInventorySlots();
    }
    
    public void SetInventorySlots()
    {
        for (int i = 0; i < invenSlots.Length; i++)
        {
            if(i < inventory.Count)
            {
                invenSlots[i].slotItem = inventory[i];
                invenSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = invenSlots[i].slotItem.amount.ToString();
            }
            else
            {
                invenSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public Item FindItemExists(int type, int grade)
    {
        foreach (var item in inventory)
        {
            if(item.type.Equals(type) && item.grade.Equals(grade))
            {
                return item;
            }
        }
        return null;
    }
    public void AddItemToInventory()
    {

    }

    public void UpdateItem()
    {

    }
}
