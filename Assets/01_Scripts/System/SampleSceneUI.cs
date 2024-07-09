using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SampleSceneUI : MonoBehaviour
{
    public Withdraw withdraw;
    public Image[] ticketIcon;

    public TextMeshProUGUI[] dungeonDescription;
    public TextMeshProUGUI ticketValue;   
    public TextMeshProUGUI timer;

    public GameObject[] dungeonEnterBtns;
    public GameObject[] dungeonLockImage;
    public GameObject characterInfo;
    public GameObject dungeonList;
    public GameObject uiList;
    public GameObject menuUi;
    public GameObject tutorialUi;
    public GameObject tutorialSkipUi;

    Stack<GameObject> uiStack = new Stack<GameObject>();

    private void Awake()
    {
        GameManager.Instance.AssignSampleSceneUi(this);
        UpdateFarmingDungeon();
    }

    void Update()
    {
        UpdateTimer();
        UpdateTicketValue();
        UpdateTicketIcon();

        // ESC 키 입력 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseTopUI();
        }
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
    void UpdateFarmingDungeon()
    {
        int clear = SheetManager.Instance.characterDatas[GameManager.Instance.selectIndex].clear;
        Debug.Log(GameManager.Instance.selectIndex);
        Debug.Log(clear);
        for (int index = 0; index < clear; index++)
        {
            dungeonEnterBtns[index].SetActive(true);
            dungeonLockImage[index].SetActive(false);
            dungeonDescription[index].text = "던전 설명 던전 설명 던전 설명 던전 설명 던전 설명";
        }
    }
   

    public void ActiveTutorialSkip()
    {
        OpenUI(tutorialSkipUi);
    }

    public void TutorialSkip()
    {
        CloseUI(tutorialSkipUi);
        tutorialUi.SetActive(false);
        SheetManager.Instance.TutorialClear();
    }

    public void CancleTutorialSkipUi()
    {
        CloseUI(tutorialSkipUi);
    }

    public void ActiveDungeonList()
    {
        AudioManager.instance.PlayUISfx(AudioManager.UISfx.dungeonList);
        // 이미 활성화 되어있다면 버튼을 누를시 꺼지게 구현
        if (dungeonList.activeSelf)
        {
            CloseUI(dungeonList);
        }
        else
        {
            OpenUI(dungeonList);           
        }
    }
    public void ClickMenu()
    {
        AudioManager.instance.PlayUISfx(AudioManager.UISfx.uiList);
        if (uiList.activeSelf)
        {
            menuUi.GetComponent<Animator>().Play("Out");
            CloseUI(uiList);
            if (dungeonList.activeSelf)
                CloseUI(dungeonList);
        }
        else
        {
            menuUi.GetComponent<Animator>().Play("In");
            OpenUI(uiList);      
        }
    }

    // UI를 스택에 추가하는 메서드
    public void OpenUI(GameObject ui)
    {
        if (!uiStack.Contains(ui))
        {
            uiStack.Push(ui);
            ui.SetActive(true);
        }
    }

    // UI를 스택에서 제거하는 메서드
    public void CloseUI(GameObject ui)
    {
        if (uiStack.Contains(ui))
        {
            uiStack = new Stack<GameObject>(new Stack<GameObject>(uiStack).Where(x => x != ui));
            ui.SetActive(false);
        }
    }

    // 스택 최상단 UI를 비활성화하는 메서드
    void CloseTopUI()
    {
        if (uiStack.Count > 1)
        {
            GameObject topUI = uiStack.Pop();
            topUI.SetActive(false);
        }
        else if(uiStack.Count == 1)
        {
            GameObject topUI = uiStack.Pop();
            topUI.SetActive(false);
            menuUi.GetComponent<Animator>().Play("Out");
        }
    }
}
