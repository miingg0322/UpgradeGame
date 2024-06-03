using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    public Image image;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.pointerId == -2)
        {
            if (item !=null)
            {
                RemoveItemFromSlot();
            }
        }
    }

    public void RegisterItemIntoSlot(Item item)
    {
        if(this.item != item)
        {
            this.item = item;
            Debug.Log($"{item.name} 등록");
        }
    }

    void RemoveItemFromSlot()
    {
        Debug.Log($"{item.name} 등록 해제");
        item = null;        
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
