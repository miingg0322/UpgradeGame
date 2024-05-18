using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleSceneUI : MonoBehaviour
{
    public Image[] ticketIcon;
    public Image playerIcon;

    public Text[] playerInfoText;
    public Text ticketValue;   
    public Text timer;

    public GameObject[] dungeonEnterBtns;
    public GameObject characterInfo;

    private void Awake()
    {
        UpdateFarminDungeon();       
    }

    private void Start()
    {
        StartCoroutine(UpdateCharacterInfo());
    }
    void Update()
    {
        UpdateTimer();
        UpdateTicketValue();
        UpdateTicketIcon();
    }
    // 입장권 충전까지 남은 시간 표시
    void UpdateTimer()
    {
        if(GameManager.Instance.dungeonTicket == GameManager.Instance.maxDungeonTicket)
        {
            timer.text = "00:00";
        }
        else
        {
            float remainTime = GameManager.Instance.ticketGenerationTime - GameManager.Instance.timer;

            if (remainTime == GameManager.Instance.ticketGenerationTime)
            {
                remainTime = 0f;
            }

            int min = Mathf.FloorToInt(remainTime / 60);
            int sec = Mathf.FloorToInt(remainTime % 60);
            timer.text = string.Format("{0:D2}:{1:D2}", min, sec);
        }
    }
    // 가진 입장권 수를 표시
    void UpdateTicketValue()
    {
        ticketValue.text = "X " + GameManager.Instance.dungeonTicket;
    }
    // 가진 입장권 수에 따라 입장권 아이콘 활성화
    void UpdateTicketIcon()
    {
        for (int index = 0; index < GameManager.Instance.maxDungeonTicket; index++)
        {
            ticketIcon[index].color = new Color(1, 1, 1, 0.2f);
        }

        for (int index = 0; index < GameManager.Instance.dungeonTicket; index++)
        {
            ticketIcon[index].color = new Color(1, 1, 1, 1);
        }
    }

    // clear 정보를 가져와서 던전을 활성화
    void UpdateFarminDungeon()
    {
        int clear = DBManager.Instance.GetBossClear(GameManager.Instance.selectedCharacterId);

        for (int index = 0; index < clear; index++)
        {
            dungeonEnterBtns[index].SetActive(true);
        }
    }
    // 캐릭터 정보 UI에 정보 할당
    IEnumerator UpdateCharacterInfo()
    {
        yield return null;

        playerIcon.sprite = Player.Instance.sprite;

        playerInfoText[0].text = "직업 : " + Player.Instance.playerName;
        playerInfoText[1].text = "HP : " + Player.Instance.maxHealth.ToString();
        playerInfoText[2].text = "공격력 : ???"; // 무기의 기본 데미지와 강화 수치를 통해 계산하도록 수정 예정
        playerInfoText[3].text = "장착중인 무기 : ???"; // 무기의 이름을 가져오도록 수정 예정
        playerInfoText[4].text = "무기 강화 수치 : ???"; // 무기의 강화 레벨을 가져오도록 수정 예정
        playerInfoText[5].text = "캐릭터 특성 : " + Player.Instance.playerTrait;
    }

    public void ActiveCharacterInfo()
    {
        // 이미 활성화 되어있다면 버튼을 누를시 꺼지게 구현
        if (characterInfo.activeSelf)
        {
            characterInfo.SetActive(false);
        }
        else
        {
            characterInfo.SetActive(true);
        }
        
    }

    public void CancleCharacterInfo()
    {
        characterInfo.SetActive(false);
    }
}
