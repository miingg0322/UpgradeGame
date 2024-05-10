using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleSceneUI : MonoBehaviour
{
    public Image[] ticketIcon;
    public Text ticketValue;   
    public Text timer;

    void Update()
    {
        UpdateTimer();
        UpdateTicketValue();
        UpdateTicketIcon();
    }

    void UpdateTimer()
    {
        float remainTime = GameManager.Instance.dungeonTicket.ticketGenerationTime - GameManager.Instance.dungeonTicket.timer;

        if (remainTime == GameManager.Instance.dungeonTicket.ticketGenerationTime)
        {
            remainTime = 0f;
        }

        int min = Mathf.FloorToInt(remainTime / 60);
        int sec = Mathf.FloorToInt(remainTime % 60);
        timer.text = string.Format("{0:D2}:{1:D2}", min, sec);
    }

    void UpdateTicketValue()
    {
        ticketValue.text = "X " + GameManager.Instance.dungeonTicket.ticket;
    }

    void UpdateTicketIcon()
    {
        for (int index = 0; index < GameManager.Instance.dungeonTicket.maxTicket; index++)
        {
            ticketIcon[index].color = new Color(1, 1, 1, 0.2f);
        }

        for (int index = 0; index < GameManager.Instance.dungeonTicket.ticket; index++)
        {
            ticketIcon[index].color = new Color(1, 1, 1, 1);
        }
    }
}
