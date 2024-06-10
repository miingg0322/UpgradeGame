using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FollowDetail : MonoBehaviour
{
    private bool isVisible = false;
    public bool IsVisible
    {
        get { return isVisible; }
        set
        {
            isVisible = value;
            gameObject.SetActive(isVisible);
        }
    }
    Image icon;
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemDetail;
    RectTransform rectTransform;
    Canvas canvas;
    Vector2 offset;
    public CustomCursor cursor;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = rectTransform.GetComponentInParent<Canvas>();
        itemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemDetail = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        offset = new Vector2(rectTransform.sizeDelta.x / 2, - rectTransform.sizeDelta.y / 2);
        gameObject.SetActive(false);
    }

    public void SetItemDetail(Item item)
    {
        itemName.text = item.name;
        itemDetail.text = $"타입: {item.type}, 등급: {item.grade}";
        if(item.type == 0)
        {
            itemDetail.text = string.Concat(itemDetail.text, $" {item.value}강");
        }
    }

    private void Update()
    {
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, cursor.transform.position, canvas.worldCamera, out movePos);

        transform.position = canvas.transform.TransformPoint(movePos + offset);
    }
}
