using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text notice;
    private void OnEnable()
    {               
        if (GameManager.Instance.isCreate)
        {
            string name = GameManager.Instance.playerData[GameManager.Instance.selectIndex].playerName;
            notice.text = $"������ \"{name}\" ĳ���͸� �����Ͻðڽ��ϱ�?";
        }
        else
        {
            int characterId = GameManager.Instance.selectIndex;
            string name = GameManager.Instance.playerData[characterId].playerName;

            notice.text = $"\"{name}\" ĳ���͸� ������ �����Ͻðڽ��ϱ�?\n������ �����ʹ� ������ �� �����ϴ�.";
        }       
    }
}
