using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SampleSceneUI : MonoBehaviour
{
    public Withdraw withdraw;
    public Image[] ticketIcon;
    public Image playerIcon;

    public Text[] playerInfoText;
    public TextMeshProUGUI[] dungeonDescription;
    public TextMeshProUGUI ticketValue;   
    public TextMeshProUGUI timer;

    public GameObject[] dungeonEnterBtns;
    public GameObject[] dungeonLockImage;
    public GameObject characterInfo;
    public GameObject settingCancleNotice;
    public GameObject settingUi;
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

    private void Start()
    {
        StartCoroutine(UpdateCharacterInfo());
    }
    void Update()
    {
        UpdateTimer();
        UpdateTicketValue();
        UpdateTicketIcon();

        // ESC Ű �Է� ó��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseTopUI();
        }
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
    void UpdateFarmingDungeon()
    {
        int clear = SheetManager.Instance.characterDatas[GameManager.Instance.selectIndex].clear;
        Debug.Log(GameManager.Instance.selectIndex);
        Debug.Log(clear);
        for (int index = 0; index < clear; index++)
        {
            dungeonEnterBtns[index].SetActive(true);
            dungeonLockImage[index].SetActive(false);
            dungeonDescription[index].text = "���� ���� ���� ���� ���� ���� ���� ���� ���� ����";
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

    public void ActiveCharacterInfo()
    {
        AudioManager.instance.PlayUISfx(AudioManager.UISfx.characterInfo);
        // �̹� Ȱ��ȭ �Ǿ��ִٸ� ��ư�� ������ ������ ����
        if (characterInfo.activeSelf)
        {
            CloseUI(characterInfo);
        }
        else
        {
            OpenUI(characterInfo);          
        }
        
    }

    public void ActiveSettingUi()
    {
        // �̹� Ȱ��ȭ �Ǿ��ִٸ� ��ư�� ������ ������ ����
        if (settingUi.activeSelf)
        {
            CloseUI(settingUi);
        }
        else
        {
            OpenUI(settingUi);
        }

    }

    public void ActiveSettingCancleNotice()
    {
        if (GameManager.Instance.isChangeSetting)
        {
            OpenUI(settingCancleNotice);
        }
        else
        {
            CloseUI(settingUi);
        }      
    }

    public void CancleSetting()
    {
        CloseUI(settingCancleNotice);
        CloseUI(settingUi);
    }
    public void CancleSettingCancleNotice()
    {
        CloseUI(settingCancleNotice);
    }
    public void CancleCharacterInfo()
    {
        CloseUI(characterInfo);
    }

    public void ActiveDungeonList()
    {
        AudioManager.instance.PlayUISfx(AudioManager.UISfx.dungeonList);
        // �̹� Ȱ��ȭ �Ǿ��ִٸ� ��ư�� ������ ������ ����
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
            if (characterInfo.activeSelf)
                CloseUI(characterInfo);
            if (withdraw.confirmUi.activeSelf)
                CloseUI(withdraw.confirmUi);
        }
        else
        {
            menuUi.GetComponent<Animator>().Play("In");
            OpenUI(uiList);      
        }
    }

    // UI�� ���ÿ� �߰��ϴ� �޼���
    public void OpenUI(GameObject ui)
    {
        if (!uiStack.Contains(ui))
        {
            uiStack.Push(ui);
            ui.SetActive(true);
        }
    }

    // UI�� ���ÿ��� �����ϴ� �޼���
    public void CloseUI(GameObject ui)
    {
        if (uiStack.Contains(ui))
        {
            uiStack = new Stack<GameObject>(new Stack<GameObject>(uiStack).Where(x => x != ui));
            ui.SetActive(false);
        }
    }

    // ���� �ֻ�� UI�� ��Ȱ��ȭ�ϴ� �޼���
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
