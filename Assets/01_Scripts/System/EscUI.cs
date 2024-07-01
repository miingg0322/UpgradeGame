using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Linq;
using UnityEngine.UI;


public class EscUI : MonoBehaviour
{
    public SceneSwitcher sceneSwitcher;
    public Image playerIcon;
    public Text[] playerInfoText;

    public GameObject escUiPanel;
    public GameObject darkOverlay;

    public GameObject returnUi;
    public GameObject settingUi;
    public GameObject settingCancleNotice;
    public GameObject characterInfoUi;
    public GameObject withdrawUi;

    public GameObject chSelectBtn;
    public GameObject logoutBtn;

    public bool notActive;

    Stack<GameObject> uiStack = new Stack<GameObject>();

    private void Start()
    {
        StartCoroutine(UpdateCharacterInfo());

        if(chSelectBtn != null && logoutBtn != null)
        {
            chSelectBtn.GetComponent<Button>().onClick.AddListener(() => sceneSwitcher.ReturnCharacterSelect());
            logoutBtn.GetComponent<Button>().onClick.AddListener(() => sceneSwitcher.Logout());
        }       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !notActive)
        {
            if (uiStack.Count == 0)
            {
                ShowUI();
            }
            else
            {
                CloseTopUI();
            }                        
        }
    }
    public void ShowUI()
    {

        OpenUI(escUiPanel);
        darkOverlay.SetActive(true);
        GameManager.Instance.isStop = true;
        Time.timeScale = 0;       
    }

    public void HideUI()
    {
        CloseUI(escUiPanel);
        darkOverlay.SetActive(false);
        GameManager.Instance.isStop = false;
        Time.timeScale = 1;
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

    public void ActiveCharacterInfo()
    {
        AudioManager.instance.PlayUISfx(AudioManager.UISfx.characterInfo);
        // �̹� Ȱ��ȭ �Ǿ��ִٸ� ��ư�� ������ ������ ����
        if (characterInfoUi.activeSelf)
        {
            CloseUI(characterInfoUi);
        }
        else
        {
            OpenUI(characterInfoUi);
        }

    }

    public void CancleCharacterInfo()
    {
        CloseUI(characterInfoUi);
    }

    public void ActiveExitFarming()
    {
        OpenUI(returnUi);
        GameManager.Instance.SaveData();
    }

    public void CancleExitFarming()
    {
        CloseUI(returnUi);
        GameManager.Instance.Init();
    }

    // UI�� ���ÿ� �߰��ϴ� �޼���
    public void OpenUI(GameObject ui)
    {
        if (!uiStack.Contains(ui))
        {
            if(uiStack.Count > 0)
            {
                // ���� �ֻ�� UI�� ��ȣ�ۿ��� ���´�
                uiStack.Peek().GetComponent<CanvasGroup>().blocksRaycasts = false;
            }

            // ���ο� UI�� ���ÿ� �߰��ϰ� ��ȣ�ۿ��� �����ϰ� �Ѵ�
            ui.GetComponent<CanvasGroup>().blocksRaycasts = true;
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

            if (uiStack.Count > 0)
            {
                GameObject topUI = uiStack.Peek();
                topUI.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
    }

    // ���� �ֻ�� UI�� ��Ȱ��ȭ�ϴ� �޼���
    void CloseTopUI()
    {
        if (uiStack.Count > 1)
        {
            GameObject topUI = uiStack.Pop();
            topUI.SetActive(false);

            if (uiStack.Count > 0)
            {
                GameObject previousTopUI = uiStack.Peek();
                previousTopUI.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }           
        }
        else if (uiStack.Count == 1)
        {
            HideUI();
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
}