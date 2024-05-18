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
    // ����� �������� ���� �ð� ǥ��
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
    // ���� ����� ���� ǥ��
    void UpdateTicketValue()
    {
        ticketValue.text = "X " + GameManager.Instance.dungeonTicket;
    }
    // ���� ����� ���� ���� ����� ������ Ȱ��ȭ
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

    // clear ������ �����ͼ� ������ Ȱ��ȭ
    void UpdateFarminDungeon()
    {
        int clear = DBManager.Instance.GetBossClear(GameManager.Instance.selectedCharacterId);

        for (int index = 0; index < clear; index++)
        {
            dungeonEnterBtns[index].SetActive(true);
        }
    }
    // ĳ���� ���� UI�� ���� �Ҵ�
    IEnumerator UpdateCharacterInfo()
    {
        yield return null;

        playerIcon.sprite = Player.Instance.sprite;

        playerInfoText[0].text = "���� : " + Player.Instance.playerName;
        playerInfoText[1].text = "HP : " + Player.Instance.maxHealth.ToString();
        playerInfoText[2].text = "���ݷ� : ???"; // ������ �⺻ �������� ��ȭ ��ġ�� ���� ����ϵ��� ���� ����
        playerInfoText[3].text = "�������� ���� : ???"; // ������ �̸��� ���������� ���� ����
        playerInfoText[4].text = "���� ��ȭ ��ġ : ???"; // ������ ��ȭ ������ ���������� ���� ����
        playerInfoText[5].text = "ĳ���� Ư�� : " + Player.Instance.playerTrait;
    }

    public void ActiveCharacterInfo()
    {
        // �̹� Ȱ��ȭ �Ǿ��ִٸ� ��ư�� ������ ������ ����
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
