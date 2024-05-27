using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvenSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public FollowDetail followDetail;
    public Item slotItem;
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
