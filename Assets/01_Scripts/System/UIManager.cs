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

        notice.text = $"\"{name}\" 캐릭터를 정말로 삭제하시겠습니까?\n삭제된 데이터는 복구할 수 없습니다.";
    }
}
