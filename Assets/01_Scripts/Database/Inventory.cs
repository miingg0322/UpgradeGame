using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    GridLayoutGroup gridGroup;
    [SerializeField]
    private int cols;
    public List<InvenSlot> invenSlots = new List<InvenSlot>();
    public GameObject slotPrefab;

    void Start()
    {
        gridGroup = GetComponent<GridLayoutGroup>();
        gridGroup.constraintCount = cols;
        invenSlots = transform.GetComponentsInChildren<InvenSlot>().ToList();

        inventory = SQLiteManager.Instance.GetAllItems();
        foreach (var item in inventory)
        {
            Debug.Log($"{item.name}::{item.type}:::{item.grade} - {item.amount}");
        }
        SetInventorySlots();
    }
    
    public void SetInventorySlots()
    {
        for (int i = 0; i < invenSlots.Count; i++)
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

    public InvenSlot GetSlotOfItem(Item item)
    {
        foreach (var invenSlot in invenSlots)
        {
            if (item == invenSlot.slotItem)
            {
                return invenSlot;
            }
        }
        return null;
    }
    public void DeleteItemFromInventory(InvenSlot slot)
    {
        invenSlots.Remove(slot);
        inventory.Remove(slot.slotItem);
        Destroy(slot.transform.parent.gameObject);
        GameObject newSlot = Instantiate(slotPrefab);
        newSlot.transform.SetParent(gridGroup.transform);
        newSlot.transform.localScale = gridGroup.transform.localScale;
        newSlot.transform.GetChild(0).gameObject.SetActive(false);
    }
}
