using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RanItemData;

public class DropItem : MonoBehaviour
{
    public Sprite[] lowGradeSprites;
    public Sprite[] midGradeSprites;
    public Sprite[] highGradeSprites;
    public Sprite[] topGradeSprites;
    public Sprite[] legendaryGradeSprites;

    public Sprite[] scrollSprites;

    public Sprite[] enhancementScrolls;

    public void ReadItemInfo(string itemInfo)
    {
        if (itemInfo.Contains("강화석"))
        {
            SetReinforcementStoneSprite(itemInfo);
        }
        else if (itemInfo.Contains("주문서"))
        {
            SetScrollSprite(itemInfo);
        }
        else if (itemInfo.Contains("강화권"))
        {
            SetEnhancementTicketSprite(itemInfo);
        }
        else if (itemInfo.Contains("null"))
        {
            return;
        }
        else
        {
            Debug.LogError("Unknown item type!");
            Debug.LogError(itemInfo);
        }
    }

    void SetReinforcementStoneSprite(string itemInfo)
    {
        string[] tokens = itemInfo.Split(' ');

        string grade = tokens[0]; // 등급
        string type = tokens[2];  // 타입

        Sprite[] sprites = null;

        switch (grade)
        {
            case "하급":
                sprites = lowGradeSprites;
                break;
            case "중급":
                sprites = midGradeSprites;
                break;
            case "상급":
                sprites = highGradeSprites;
                break;
            case "최상급":
                sprites = topGradeSprites;
                break;
            case "전설급":
                sprites = legendaryGradeSprites;
                break;
            default:
                Debug.LogError("Invalid grade!");
                return;
        }

        Sprite selectedSprite = null;

        switch (type)
        {
            case "A":
                selectedSprite = sprites[0];
                break;
            case "B":
                selectedSprite = sprites[1];
                break;
            case "C":
                selectedSprite = sprites[2];
                break;
            case "D":
                selectedSprite = sprites[3];
                break;
            default:
                Debug.LogError("Invalid type!");
                return;
        }

        if (selectedSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = selectedSprite;
        }
        else
        {
            Debug.LogError("No matching sprite found!");
        }      
    }

    void SetScrollSprite(string itemInfo)
    {
        string[] tokens = itemInfo.Split(' ');

        string percentage = tokens[3];

        Sprite selectedSprite = null;

        switch (percentage)
        {
            case "3%":
                selectedSprite = scrollSprites[0];
                break;
            case "5%":
                selectedSprite = scrollSprites[1];
                break;
            case "10%":
                selectedSprite = scrollSprites[2];
                break;
        }

        if (selectedSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = selectedSprite;
        }
        else
        {
            Debug.LogError("No matching sprite found!");
        }
    }

    void SetEnhancementTicketSprite(string itemInfo)
    {
        string[] tokens = itemInfo.Split(' ');

        string enhancementLevelString = "";
        
        foreach (char c in tokens[2])
        {
            if (char.IsDigit(c))
            {
                enhancementLevelString += c;
            }
        }

        int enhancementLevel;
        if (!int.TryParse(enhancementLevelString, out enhancementLevel))
        {
            Debug.LogError("Invalid enhancement level!");
            return;
        }

        Sprite selectedSprite = null;

        if (enhancementLevel >= 0 && enhancementLevel <= 10)
        {
            selectedSprite = enhancementScrolls[0];
        }
        else if (enhancementLevel >= 0 && enhancementLevel <= 15)
        {
            selectedSprite = enhancementScrolls[1];
        }
        else if (enhancementLevel >= 0 && enhancementLevel <= 20)
        {
            selectedSprite = enhancementScrolls[2];
        }

        if (selectedSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = selectedSprite;
        }
        else
        {
            Debug.LogError("No matching sprite found!");
        }
    }
}
        
