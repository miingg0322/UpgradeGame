using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleSceneUI : MonoBehaviour
{
    public Image[] ticketIcon;
    public Text ticketValue;   
    public Text timer;

    public GameObject[] dungeonEnterBtns;

    private void Awake()
    {
        UpdateFarminDungeon();
    }
    void Update()
    {
        UpdateTimer();
        UpdateTicketValue();
        UpdateTicketIcon();
    }

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

    void UpdateTicketValue()
    {
        ticketValue.text = "X " + GameManager.Instance.dungeonTicket;
    }

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

    void UpdateFarminDungeon()
    {
        int clear = DBManager.Instance.GetBossClear(GameManager.Instance.selectedCharacterId);

        for (int index = 0; index < clear; index++)
        {
            dungeonEnterBtns[index].SetActive(true);
        }
    }
}
