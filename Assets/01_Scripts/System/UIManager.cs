using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text notice;
    private void OnEnable()
    {
        int characterId = GameManager.Instance.deleteCharacter;
        int classId = DBManager.Instance.GetCharacterClass(characterId);
        string name = GameManager.Instance.playerData[classId].playerName;

        notice.text = $"\"{name}\" ĳ���͸� ������ �����Ͻðڽ��ϱ�?\n������ �����ʹ� ������ �� �����ϴ�.";
    }
}
