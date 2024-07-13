using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Linq;
using UnityEngine.UI;
using TMPro;


public class EscUI : MonoBehaviour
{
    public GameObject soundsetUI;
    public GameObject keysetUI;

    public SceneSwitcher sceneSwitcher;
    public Image playerIcon;
    public Text[] playerInfoText;

    public GameObject escUiPanel;
    public GameObject darkOverlay;

    public GameObject returnUi;
    public GameObject settingUi;   
    public GameObject characterInfoUi;
    public GameObject withdrawUi;

    public GameObject settingCancleNotice;
    public GameObject chSelectNotice;
    public GameObject logoutNotice;

    public GameObject chSelectBtn;
    public GameObject logoutBtn;
    public GameObject soundsetUiBtn;
    public GameObject keysetUiBtn;
    public GameObject soundSetDarkOverlay;
    public GameObject keySetDarkOverlay;

    public bool notActive;
    public TextMeshProUGUI[] keyText;

    Stack<GameObject> uiStack = new Stack<GameObject>();

    private void Start()
    {
        StartCoroutine(UpdateCharacterInfo());

        if(chSelectBtn != null && logoutBtn != null)
        {
            chSelectBtn.GetComponent<Button>().onClick.AddListener(() => sceneSwitcher.ReturnCharacterSelect());
            logoutBtn.GetComponent<Button>().onClick.AddListener(() => sceneSwitcher.Logout());
        }
        UpdateKeyText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !notActive && !GameManager.Instance.isKeySetting && !GameManager.Instance.isDungeonClear)
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
        UpdateKeyText();
    }

    void UpdateKeyText()
    {
        for (int index = 0; index < keyText.Length; index++)
        {
            KeyCode key = KeySetting.keys[(KeyAction)index];
            string keyString = key.ToUserFriendlyString();

            keyText[index].text = keyString;

            // ����Ű�� ũ�� Ű���
            if (key == KeyCode.UpArrow || key == KeyCode.DownArrow || key == KeyCode.LeftArrow || key == KeyCode.RightArrow)
            {
                keyText[index].fontSize = 45; // ������ ũ��
            }
            else
            {
                keyText[index].fontSize = 24; // �⺻ ũ��
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

    public void ShowSoundSetUI()
    {
        OpenUI(soundsetUI);
        CloseUI(keysetUI);
        soundSetDarkOverlay.SetActive(true);
        keySetDarkOverlay.SetActive(false);
    }

    public void ShowKeySetUI()
    {
        OpenUI(keysetUI);
        CloseUI(soundsetUI);
        soundSetDarkOverlay.SetActive(false);
        keySetDarkOverlay.SetActive(true);
    }
    public void ActiveSettingUi()
    {
        // �̹� Ȱ��ȭ �Ǿ��ִٸ� ��ư�� ������ ������ ����
        if (settingUi.activeSelf)
        {
            CloseUI(settingUi);
            soundsetUiBtn.SetActive(false);
            keysetUiBtn.SetActive(false);
        }
        else
        {
            OpenUI(settingUi);
            soundsetUiBtn.SetActive(true);
            keysetUiBtn.SetActive(true);
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
            CloseUI(soundsetUI);
            CloseUI(keysetUI);
            soundsetUiBtn.SetActive(false);
            keysetUiBtn.SetActive(false);
        }
    }

    public void CancleSetting()
    {
        CloseUI(settingCancleNotice);
        CloseUI(soundsetUI);
        CloseUI(keysetUI);
        soundsetUiBtn.SetActive(false);
        keysetUiBtn.SetActive(false);
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

    public void ShowChSelectNotice()
    {
        OpenUI(chSelectNotice);
    }

    public void CancleChSelectNotice()
    {
        CloseUI(chSelectNotice); 
    }

    public void ShowLogoutNotice()
    {
        OpenUI(logoutNotice);
    }

    public void CancleLogoutNotice()
    {
        CloseUI(logoutNotice);
    }
    public void ActiveExitFarming()
    {
        OpenUI(returnUi);
        //GameManager.Instance.SaveData();
    }

    public void CancleExitFarming()
    {
        CloseUI(returnUi);
        //GameManager.Instance.Init();
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

            if(topUI.gameObject.name == "Sound Panel" || topUI.gameObject.name == "Keyset Panel")
            {
                soundsetUiBtn.SetActive(false);
                keysetUiBtn.SetActive(false);
            }

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

public static class KeyCodeExtensions
{
    public static string ToUserFriendlyString(this KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.Alpha0: return "0";
            case KeyCode.Alpha1: return "1";
            case KeyCode.Alpha2: return "2";
            case KeyCode.Alpha3: return "3";
            case KeyCode.Alpha4: return "4";
            case KeyCode.Alpha5: return "5";
            case KeyCode.Alpha6: return "6";
            case KeyCode.Alpha7: return "7";
            case KeyCode.Alpha8: return "8";
            case KeyCode.Alpha9: return "9";
            case KeyCode.Mouse0: return "Left Click";
            case KeyCode.Mouse1: return "Right Click";
            case KeyCode.Mouse2: return "Mouse Wheel";
            case KeyCode.Space: return "SpaceBar";
            case KeyCode.Return: return "Enter";
            case KeyCode.BackQuote: return "`";
            case KeyCode.LeftControl: return "Left Ctrl";
            case KeyCode.RightControl: return "Right Ctrl";
            case KeyCode.Backslash: return "|";
            case KeyCode.UpArrow: return "��";
            case KeyCode.DownArrow: return "��";
            case KeyCode.LeftArrow: return "��";
            case KeyCode.RightArrow: return "��";
            case KeyCode.LeftBracket: return "[";
            case KeyCode.RightBracket: return "]";
            case KeyCode.Minus: return "-";
            case KeyCode.Equals: return "=";
            case KeyCode.Semicolon: return ";";
            case KeyCode.Quote: return "'";
            case KeyCode.Comma: return ",";
            case KeyCode.Period: return ".";
            case KeyCode.Slash: return "/";

            default: return keyCode.ToString();
        }
    }
}
