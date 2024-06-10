using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InvenSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public FollowDetail followDetail;
    public Item slotItem;
    public TMP_Text tmpText;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.pointerId == -2)
        {
            if (slotItem.type.Equals(0))
            {
                WeaponSlot slot = FindAnyObjectByType<WeaponSlot>();
                if (slot!=null)
                {
                    slot.RegisterItemIntoSlot(slotItem);
                    slot.weapon.SetWeaponData((int)SQLiteManager.Instance.playingCharacter.charClass, slotItem.grade);
                    slot.weapon.Level = slotItem.value;
                }
                else
                {
                    if (!GameManager.Instance.player.weapon.name.Equals(slotItem.name))
                    {
                        // 무기 변경
                        
                    }
                }
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

    public void SetItemAmountText(int amount)
    {
        if(amount == 0)
        {
            SQLiteManager.Instance.inventory.DeleteItemFromInventory(this);
        }
        else
        {
            tmpText.text = amount.ToString();
        }
    }
}
