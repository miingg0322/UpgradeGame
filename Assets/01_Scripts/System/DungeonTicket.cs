using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonTicket : MonoBehaviour
{
    public int ticket;
    public int maxTicket;
    public float ticketGenerationTime;
    public float timer;

    void Awake()
    {
        GameManager.Instance.AssignTicket(this);        
    }
    void Update()
    {
        if (ticket != maxTicket)
        {
            timer += Time.deltaTime;              
        }
                      
        if (timer >= ticketGenerationTime)
        {
            timer = 0f;
            IncreaseTicket();
        }
    }

    void IncreaseTicket()
    {
        if (ticket < maxTicket)
        {
            ticket++;
            Debug.Log("던전 입장권이 자동으로 생성되었습니다. 현재 입장권 개수: " + ticket);
        }
    }

    public void Init()
    {
        int characterId = GameManager.Instance.selectedCharacterId;

        int saveTicket = PlayerPrefs.GetInt("Character" + characterId + "Ticket", maxTicket);
        float saveTimer = PlayerPrefs.GetFloat("Character" + characterId + "Timer", 0);
        long saveTime = Convert.ToInt64(PlayerPrefs.GetString("Character" + characterId + "Time", "0"));

        if (saveTicket == maxTicket)
        {
            ticket = maxTicket;
            timer = 0f;
            return;
        }
        else
        {
            TimeSpan timePassed = DateTime.Now - new DateTime(saveTime);
            float elapsedTime = (float)timePassed.TotalSeconds;
            int increasedTickets = (int)(elapsedTime / ticketGenerationTime);
            saveTicket += increasedTickets;           
            saveTimer += (long)(elapsedTime % ticketGenerationTime);

            if (saveTicket >= maxTicket)
            {
                saveTicket = maxTicket;
                timer = 0f;
            }
                
            ticket = saveTicket;
            timer = Mathf.Min(saveTimer, ticketGenerationTime);
        }                 
    }

    public void SaveData()
    {
        int characterId = GameManager.Instance.selectedCharacterId;

        PlayerPrefs.SetInt("Character" + characterId + "Ticket", ticket);
        PlayerPrefs.SetFloat("Character" + characterId + "Timer", timer);
        PlayerPrefs.SetString("Character" + characterId + "Time", DateTime.Now.Ticks.ToString()); 
    }
}
