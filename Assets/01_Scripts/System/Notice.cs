using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Notice : MonoBehaviour
{
    public GameObject noticeUi;
    public GameObject clearlUi;
    public GameObject specialMoveBtn;
    public GameObject cleaner;
    public Image noticeIcon;
    public Text noticeText;
    public Text ticketText;
    public Text timerText;

    public GameObject[] slots;
    public Image[] itemIcons;
    public Text[] itemNames;
    public Text[] itemGrades;
    public Text[] itemQuantitys;

    public GameObject[] testClearBtns;
    private void Awake()
    {
        GameManager.Instance.AssignNotice(this);
        GameManager.Instance.isDungeonClear = false;
    }

    private void Start()
    {
        for (int index = 0; index < testClearBtns.Length; index++)
        {
            int selectIndex = index;
            testClearBtns[selectIndex].GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.BossClear(selectIndex + 1));
        }
        specialMoveBtn.GetComponent<Button>().onClick.AddListener(() => Player.Instance.SpecialMove());
    }

    private void Update()
    {
        UpdateTimer();
        UpdateTicketValue();
    }

    void UpdateTimer()
    {
        if (GameManager.Instance.dungeonTicket == GameManager.Instance.maxDungeonTicket)
        {
            timerText.text = "00:00";
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
            timerText.text = string.Format("{0:D2}:{1:D2}", min, sec);
        }
    }

    void UpdateTicketValue()
    {
        ticketText.text = "���� ����� : " + GameManager.Instance.dungeonTicket + "��";
    }

    public void DungeonClear()
    {
        AudioManager.instance.PlaySkillSfx(AudioManager.SkillSfx.victory);
        // ������ ȹ�� �˸� ��Ȱ��ȭ
        noticeUi.SetActive(false);
        // ���� ���� ���� û��
        cleaner.SetActive(true);

        Invoke("ShowClearUI", 3f);
    }

    public void ShowClearUI()
    {
 
        cleaner.SetActive(false);
        clearlUi.SetActive(true);

        // �����ص� ������ ��������
        List<CollectItem> collectItems = new List<CollectItem>();
        collectItems = GameManager.Instance.collectedItems;

        // ui �ʱ�ȭ
        ClearSlotInit();

        // UI�� �����ص� �������� ���� �Ҵ�
        for (int index = 0; index < collectItems.Count; index++)
        {
            if (index == slots.Length)
                break;

            slots[index].SetActive(true);
            itemIcons[index].sprite = collectItems[index].itemImage;
            itemNames[index].text = collectItems[index].itemName;
            itemGrades[index].text = collectItems[index].itemGrade;

            // rare ����̸� �ؽ�Ʈ�� ����������� ����
            if (itemGrades[index].text == "rare")
            {
                itemGrades[index].GetComponent<Text>().color = new Color(190f / 255f, 110f / 255f, 236f / 255f);
            }

            itemQuantitys[index].text = collectItems[index].itemQuantity.ToString();
        }
    }
    public void NoticeRoutine()
    {
        noticeUi.SetActive(true);
        noticeUi.GetComponent<Animator>().SetTrigger("Notice");

        Invoke("HideNotice", 4f);       
    }

    public void HideNotice()
    {
        noticeUi.SetActive(false);
    }

    public void ClearSlotInit()
    {
        GameManager.Instance.CollectedItemsInit();

        for(int index = 0; index < slots.Length; index++)
        {
            if (slots[index].activeSelf)
            {                
                // rare ����̸� �ؽ�Ʈ�� ��������� ����
                if (itemGrades[index].text == "rare")
                {
                    itemGrades[index].GetComponent<Text>().color = new Color(245f / 255f, 174f / 255f, 24f / 255f);
                }

                itemIcons[index].sprite = null;
                itemNames[index].text = "";
                itemGrades[index].text = "";
                itemQuantitys[index].text = "0";

                slots[index].SetActive(false);
            }
        }
    }
    public void ActiveSpecialMove()
    {
        specialMoveBtn.SetActive(true);
    }
}
