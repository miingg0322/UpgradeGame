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
        // 이미 활성화 되어있다면 버튼을 누를시 꺼지게 구현
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
        // 이미 활성화 되어있다면 버튼을 누를시 꺼지게 구현
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

    // UI를 스택에 추가하는 메서드
    public void OpenUI(GameObject ui)
    {
        if (!uiStack.Contains(ui))
        {
            if(uiStack.Count > 0)
            {
                // 현재 최상단 UI의 상호작용을 막는다
                uiStack.Peek().GetComponent<CanvasGroup>().blocksRaycasts = false;
            }

            // 새로운 UI를 스택에 추가하고 상호작용을 가능하게 한다
            ui.GetComponent<CanvasGroup>().blocksRaycasts = true;
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

            if (uiStack.Count > 0)
            {
                GameObject topUI = uiStack.Peek();
                topUI.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
    }

    // 스택 최상단 UI를 비활성화하는 메서드
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
}
