using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvenSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public FollowDetail followDetail;
    public Item slotItem;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.pointerId == -2)
        {
            WeaponSlot slot = FindAnyObjectByType<WeaponSlot>();
            if (slot && slotItem.type.Equals(0))
            {
                slot.RegisterItemIntoSlot(slotItem);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        followDetail.IsVisible = true;
        followDetail.SetItemDetail(slotItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        followDetail.IsVisible = false;
    }
}
