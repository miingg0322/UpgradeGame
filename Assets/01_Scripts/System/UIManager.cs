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
            notice.text = $"정말로 \"{name}\" 캐릭터를 생성하시겠습니까?";
        }
        else
        {
            int characterId = GameManager.Instance.selectIndex;
            string name = GameManager.Instance.playerData[characterId].playerName;

            notice.text = $"\"{name}\" 캐릭터를 정말로 삭제하시겠습니까?\n삭제된 데이터는 복구할 수 없습니다.";
        }       
    }
}
