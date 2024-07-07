using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum KeyAction { UP, DOWN, LEFT, RIGHT, DASH, SKILL, ATTACK, KEYCOUNT }

public static class KeySetting { public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>(); }
public class SettingManager : MonoBehaviour
{   
    [Header("#Sound Setting")]
    public Slider sensitivitySlider;
    public Slider volumeSlider;
    public Slider bgmVolumeSlider;
    public Slider gameVolumeSlider;
    public Slider skillVolumeSlider;
    public Slider uiVolumeSlider;

    public Image volumeSliderHandle;
    public Image bgmVolumeSliderHandle;
    public Image gameVolumeSliderHandle;
    public Image skillVolumeSliderHandle;
    public Image uiVolumeSliderHandle;

    public TMP_InputField sensitivityValue;
    public TMP_InputField soundValue;
    public TMP_InputField bgmSoundValue;
    public TMP_InputField gameSoundValue;
    public TMP_InputField skillSoundValue;
    public TMP_InputField uiSoundValue;

    public Toggle totalSoundToggle;
    public Toggle bgmSoundToggle;
    public Toggle gameSoundToggle;
    public Toggle skillSoundToggle;
    public Toggle uiSoundToggle;

    public CustomCursor customCursor;

    [Header("#Key Setting")]
    public GameObject[] keySettingBtn;

    bool isChanging;

    int basicVolumeValue = 70;
    int basicSensitivityValue = 50;
    int maxVolume;

    int codeKey = -1;

    KeyCode[] defaultKeys = new KeyCode[] { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.LeftShift, KeyCode.R, KeyCode.Mouse0 };
    private void Awake()
    {
        InitKeySetting();
    }
    void Start()
    {
        // 전체 사운드 Value 값 설정
        maxVolume = PlayerPrefs.HasKey("volumeValue") ? PlayerPrefs.GetInt("volumeValue") : basicVolumeValue;

        InitSlider();
        InitToggle();       
    }

    private void OnGUI()
    {
        Event keyEvent = Event.current;

        if (codeKey != -1)
        {
            GameManager.Instance.isKeySetting = true;
            keySettingBtn[codeKey].GetComponent<Button>().interactable = false;
            if (keyEvent.isKey)
            {
                KeyCode newKey = keyEvent.keyCode;
                HandleKeyChange(newKey);
            }
            else if (keyEvent.type == EventType.MouseDown)
            {
                KeyCode newMouseKey = KeyCode.None;

                switch (keyEvent.button)
                {
                    case 0:
                        newMouseKey = KeyCode.Mouse0;
                        break;
                    case 1:
                        newMouseKey = KeyCode.Mouse1;
                        break;
                    case 2:
                        newMouseKey = KeyCode.Mouse2;
                        break;
                }

                if (newMouseKey != KeyCode.None)
                {
                    HandleKeyChange(newMouseKey);
                }
            }
        }
    }
    private void HandleKeyChange(KeyCode newKey)
    {
        KeyAction existingAction = KeyAction.KEYCOUNT;

        foreach (var entry in KeySetting.keys)
        {
            if (entry.Value == newKey)
            {
                existingAction = entry.Key;
                break;
            }
        }

        if (existingAction != KeyAction.KEYCOUNT)
        {
            KeySetting.keys[existingAction] = KeySetting.keys[(KeyAction)codeKey];
        }

        KeySetting.keys[(KeyAction)codeKey] = newKey;
        StartCoroutine(EndKeySetting());
        keySettingBtn[codeKey].GetComponent<Button>().interactable = true;
        codeKey = -1;
        CheckChangeSetting();
    }

    IEnumerator EndKeySetting()
    {
        yield return null;

        GameManager.Instance.isKeySetting = false;
    }
    public void ChangeKey(int num)
    {
        codeKey = num;
    }
    void InitSlider()
    {
        // 슬라이더의 On Value Changed에 메서드 등록
        sensitivitySlider.onValueChanged.AddListener(delegate { OnSensitivityChanged(); });
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(); });
        bgmVolumeSlider.onValueChanged.AddListener(delegate { OnBGMVolumeChanged(); });
        gameVolumeSlider.onValueChanged.AddListener(delegate { OnGameVolumeChanged(); });
        skillVolumeSlider.onValueChanged.AddListener(delegate { OnSkillVolumeChanged(); });
        uiVolumeSlider.onValueChanged.AddListener(delegate { OnUIVolumeChanged(); });

        // 슬라이더와 InputField에 저장된 값 or 기본 값 할당
        volumeSlider.value = maxVolume;
        soundValue.text = maxVolume.ToString();
        bgmVolumeSlider.value = PlayerPrefs.HasKey("bgmVolumeValue") ? Mathf.Min(PlayerPrefs.GetInt("bgmVolumeValue"), maxVolume) : maxVolume;
        bgmSoundValue.text = bgmVolumeSlider.value.ToString();
        gameVolumeSlider.value = PlayerPrefs.HasKey("gameVolumeValue") ? Mathf.Min(PlayerPrefs.GetInt("gameVolumeValue"), maxVolume) : maxVolume;
        gameSoundValue.text = gameVolumeSlider.value.ToString();
        skillVolumeSlider.value = PlayerPrefs.HasKey("skillVolumeValue") ? Mathf.Min(PlayerPrefs.GetInt("skillVolumeValue"), maxVolume) : maxVolume;
        skillSoundValue.text = skillVolumeSlider.value.ToString();
        uiVolumeSlider.value = PlayerPrefs.HasKey("uiVolumeValue") ? Mathf.Min(PlayerPrefs.GetInt("uiVolumeValue"), maxVolume) : maxVolume;
        uiSoundValue.text = uiVolumeSlider.value.ToString();
        sensitivitySlider.value = PlayerPrefs.HasKey("cursorSensitivity") ? PlayerPrefs.GetInt("cursorSensitivity") : basicSensitivityValue;
        sensitivityValue.text = sensitivitySlider.value.ToString();
    }
    void InitToggle()
    {
        // Toggle 초기상태 설정
        totalSoundToggle.isOn = PlayerPrefs.HasKey("isMuteTotalSound") ? (PlayerPrefs.GetInt("isMuteTotalSound") == 1 ? true : false) : false;
        bgmSoundToggle.isOn = PlayerPrefs.HasKey("isMuteBgmSound") ? (PlayerPrefs.GetInt("isMuteBgmSound") == 1 ? true : false) : false;
        gameSoundToggle.isOn = PlayerPrefs.HasKey("isMuteGameSound") ? (PlayerPrefs.GetInt("isMuteGameSound") == 1 ? true : false) : false;
        skillSoundToggle.isOn = PlayerPrefs.HasKey("isMuteSkillSound") ? (PlayerPrefs.GetInt("isMuteSkillSound") == 1 ? true : false) : false;
        uiSoundToggle.isOn = PlayerPrefs.HasKey("isMuteUiSound") ? (PlayerPrefs.GetInt("isMuteUiSound") == 1 ? true : false) : false;

        SetTotalToggle();
        SetBgmToggle();
        SetGameToggle();
        SetSkillToggle();
        SetUIToggle();
    }

    void InitKeySetting()
    {
        // 저장된 값이 있는지 확인하고 있다면 그 값을 사용하여 초기화
        foreach (KeyAction keyAction in System.Enum.GetValues(typeof(KeyAction)))
        {
            if (keyAction == KeyAction.KEYCOUNT) continue;

            string keyString = PlayerPrefs.GetString(keyAction.ToString(), null);
            if (!string.IsNullOrEmpty(keyString))
            {
                KeyCode loadedKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyString);
                KeySetting.keys[keyAction] = loadedKey;
            }
            else
            {
                KeySetting.keys[keyAction] = defaultKeys[(int)keyAction];
            }
        }
    }
    void OnVolumeChanged()
    {
        // 음량을 슬라이더의 값에 따라 조절
        float volume = volumeSlider.value / 100f;
        soundValue.text = Mathf.FloorToInt(volumeSlider.value).ToString();

        // 음소거 상태일때는 전체 사운드가 0이 되게 설정
        if (totalSoundToggle.isOn)
        {
            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = volume;
        }       
        UpdateVolumeSliderMax();
        CheckChangeSetting();
    }

    // 사용자가 텍스트 필드에 값을 입력할 때마다 호출되는 함수
    public void OnInputValueChanged()
    {
        // 텍스트 필드의 값을 읽어와 슬라이더의 값으로 설정
        float value = volumeSlider.value;
        if (float.TryParse(soundValue.text, out value)) // 입력된 값이 float 값이여야 변경
        {
            value = Mathf.Clamp(value, 0f, 100f);
            volumeSlider.value = value;
        }
        UpdateVolumeSliderMax();
        CheckChangeSetting();
    }

    // Total Sound의 음소거 Toggle을 누를때 호출되는 함수
    public void MuteTotalSound()
    {
        if (totalSoundToggle.isOn) // 토글이 켜져있다면 꺼지도록 설정
        {
            SetMuteTotalSound();
        }
        else // 토글이 꺼져있다면 켜지도록 설정
        {
            SetActiveTotalSound();          
        }
        CheckChangeSetting();
    }

    void SetMuteTotalSound()
    {
        // 슬라이더가 회색으로 비활성화 된 이미지로 수정
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(80, 80, 80, 255);

        volumeSlider.colors = colors;
        bgmVolumeSlider.colors = colors;
        gameVolumeSlider.colors = colors;
        skillVolumeSlider.colors = colors;
        uiVolumeSlider.colors = colors;

        volumeSliderHandle.color = new Color32(80, 80, 80, 255);
        bgmVolumeSliderHandle.color = new Color32(80, 80, 80, 255);
        gameVolumeSliderHandle.color = new Color32(80, 80, 80, 255);
        skillVolumeSliderHandle.color = new Color32(80, 80, 80, 255);
        uiVolumeSliderHandle.color = new Color32(80, 80, 80, 255);

        // 사운드 0으로 설정
        AudioListener.volume = 0f;
    }

    void SetActiveTotalSound()
    {
        // 슬라이더가 원래의 색으로 활성화 된 이미지로 수정
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(255, 255, 255, 255);
        volumeSlider.colors = colors;     
        volumeSliderHandle.color = new Color32(255, 255, 255, 255);

        // BGM이 음소거 상태면 실행되지 않게 구현
        if(!bgmSoundToggle.isOn)
        {
            bgmVolumeSlider.colors = colors;
            bgmVolumeSliderHandle.color = new Color32(255, 255, 255, 255);
        }
        // Game 사운드가 음소거 상태면 실행되지 않게 구현
        if (!gameSoundToggle.isOn)
        {
            gameVolumeSlider.colors = colors;
            gameVolumeSliderHandle.color = new Color32(255, 255, 255, 255);
        }
        // Skill 사운드가 음소거 상태면 실행되지 않게 구현
        if (!skillSoundToggle.isOn)
        {
            skillVolumeSlider.colors = colors;
            skillVolumeSliderHandle.color = new Color32(255, 255, 255, 255);
        }
        // UI 사운드가 음소거 상태면 실행되지 않게 구현
        if (!uiSoundToggle.isOn)
        {
            uiVolumeSlider.colors = colors;
            uiVolumeSliderHandle.color = new Color32(255, 255, 255, 255);
        }

        // Volume에 설정된 값으로 할당
        float volume = volumeSlider.value / 100f;
        AudioListener.volume = volume;
    }

    // Total Sound의 음소거 Toggle을 누를때 호출되는 함수
    public void MuteBgmSound()
    {
        if (totalSoundToggle.isOn)
            return;

        if (bgmSoundToggle.isOn) // 토글이 켜져있다면 꺼지도록 설정
        {
            SetMuteBgmSound();
        }
        else // 토글이 꺼져있다면 켜지도록 설정
        {
            SetActiveBgmSound();
        }

        if (gameSoundToggle.isOn) // 토글이 켜져있다면 꺼지도록 설정
        {
            SetMuteGameSound();
        }
        else // 토글이 꺼져있다면 켜지도록 설정
        {
            SetActiveGameSound();
        }

        if (skillSoundToggle.isOn) // 토글이 켜져있다면 꺼지도록 설정
        {
            SetMuteSkillSound();
        }
        else // 토글이 꺼져있다면 켜지도록 설정
        {
            SetActiveSkillSound();
        }
        if (uiSoundToggle.isOn) // 토글이 켜져있다면 꺼지도록 설정
        {
            SetMuteUISound();
        }
        else // 토글이 꺼져있다면 켜지도록 설정
        {
            SetActiveUISound();
        }
        CheckChangeSetting();
    }

    void SetMuteBgmSound()
    {
        // 슬라이더가 회색으로 비활성화 된 이미지로 수정
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(80, 80, 80, 255);
        bgmVolumeSlider.colors = colors;
        bgmVolumeSliderHandle.color = new Color32(80, 80, 80, 255);

        // 사운드 0으로 설정
        AudioManager.Instance.BGMVolumeSetting(0);
    }

    void SetActiveBgmSound()
    {
        // 슬라이더가 원래의 색으로 활성화 된 이미지로 수정
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(255, 255, 255, 255);
        bgmVolumeSlider.colors = colors;
        bgmVolumeSliderHandle.color = new Color32(255, 255, 255, 255);

        // Volume에 설정된 값으로 할당
        float volume = bgmVolumeSlider.value / 100f;
        AudioManager.Instance.BGMVolumeSetting(volume);
    }

    void OnBGMVolumeChanged()
    {
        if (bgmVolumeSlider.value > volumeSlider.value) // Total Sound보다 작을때만 수정되도록 구현
        {
            bgmVolumeSlider.value = volumeSlider.value;
            float volume = bgmVolumeSlider.value / 100;
            bgmSoundValue.text = Mathf.FloorToInt(bgmVolumeSlider.value).ToString();
            AudioManager.Instance.BGMVolumeSetting(volume);
        }
        else
        {
            // 음량을 슬라이더의 값에 따라 조절
            float volume = bgmVolumeSlider.value / 100;
            bgmSoundValue.text = Mathf.FloorToInt(bgmVolumeSlider.value).ToString();
            AudioManager.Instance.BGMVolumeSetting(volume);
        }
        CheckChangeSetting();
    }

    // 사용자가 텍스트 필드에 값을 입력할 때마다 호출되는 함수
    public void OnInputBGMValueChanged()
    {
        // 텍스트 필드의 값을 읽어와 슬라이더의 값으로 설정
        float value = bgmVolumeSlider.value;
        if (float.TryParse(bgmSoundValue.text, out value))
        {
            value = Mathf.Clamp(value, 0f, volumeSlider.value);
            bgmVolumeSlider.value = value;
        }
        CheckChangeSetting();
    }
    public void MuteGameSound()
    {
        if (totalSoundToggle.isOn)
            return;

        if (gameSoundToggle.isOn) // 토글이 켜져있다면 꺼지도록 설정
        {
            SetMuteGameSound();
        }
        else // 토글이 꺼져있다면 켜지도록 설정
        {
            SetActiveGameSound();
        }
        CheckChangeSetting();
    }

    void SetMuteGameSound()
    {
        // 슬라이더가 회색으로 비활성화 된 이미지로 수정
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(80, 80, 80, 255);
        gameVolumeSlider.colors = colors;
        gameVolumeSliderHandle.color = new Color32(80, 80, 80, 255);

        // 사운드 0으로 설정
        AudioManager.Instance.GameVolumeSetting(0);
    }

    void SetActiveGameSound()
    {
        // 슬라이더가 원래의 색으로 활성화 된 이미지로 수정
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(255, 255, 255, 255);
        gameVolumeSlider.colors = colors;
        gameVolumeSliderHandle.color = new Color32(255, 255, 255, 255);

        // Volume에 설정된 값으로 할당
        float volume = gameVolumeSlider.value / 100f;
        AudioManager.Instance.GameVolumeSetting(volume);
    }
    public void OnGameVolumeChanged()
    {
        if (gameVolumeSlider.value > volumeSlider.value) // Total Sound보다 작을때만 수정되도록 구현
        {
            gameVolumeSlider.value = volumeSlider.value;
            float volume = gameVolumeSlider.value / 100;
            gameSoundValue.text = Mathf.FloorToInt(gameVolumeSlider.value).ToString();
            AudioManager.Instance.GameVolumeSetting(volume);
        }
        else
        {
            // 음량을 슬라이더의 값에 따라 조절
            float volume = gameVolumeSlider.value / 100;
            gameSoundValue.text = Mathf.FloorToInt(gameVolumeSlider.value).ToString();
            AudioManager.Instance.GameVolumeSetting(volume);
        }
        CheckChangeSetting();
    }
    public void OnInputGameValueChanged()
    {
        // 텍스트 필드의 값을 읽어와 슬라이더의 값으로 설정
        float value = gameVolumeSlider.value;
        if (float.TryParse(gameSoundValue.text, out value))
        {
            value = Mathf.Clamp(value, 0f, volumeSlider.value);
            gameVolumeSlider.value = value;
        }
        CheckChangeSetting();
    }

    public void MuteSkillSound()
    {
        if (totalSoundToggle.isOn)
            return;

        if (skillSoundToggle.isOn) // 토글이 켜져있다면 꺼지도록 설정
        {
            SetMuteSkillSound();
        }
        else // 토글이 꺼져있다면 켜지도록 설정
        {
            SetActiveSkillSound();
        }
        CheckChangeSetting();
    }

    void SetMuteSkillSound()
    {
        // 슬라이더가 회색으로 비활성화 된 이미지로 수정
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(80, 80, 80, 255);
        skillVolumeSlider.colors = colors;
        skillVolumeSliderHandle.color = new Color32(80, 80, 80, 255);

        // 사운드 0으로 설정
        AudioManager.Instance.SkillVolumeSetting(0);
    }

    void SetActiveSkillSound()
    {
        // 슬라이더가 원래의 색으로 활성화 된 이미지로 수정
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(255, 255, 255, 255);
        skillVolumeSlider.colors = colors;
        skillVolumeSliderHandle.color = new Color32(255, 255, 255, 255);

        // Volume에 설정된 값으로 할당
        float volume = skillVolumeSlider.value / 100f;
        AudioManager.Instance.SkillVolumeSetting(volume);
    }
    public void OnSkillVolumeChanged()
    {
        if (skillVolumeSlider.value > volumeSlider.value) // Total Sound보다 작을때만 수정되도록 구현
        {
            skillVolumeSlider.value = volumeSlider.value;
            float volume = skillVolumeSlider.value / 100;
            skillSoundValue.text = Mathf.FloorToInt(skillVolumeSlider.value).ToString();
            AudioManager.Instance.SkillVolumeSetting(volume);
        }
        else
        {
            // 음량을 슬라이더의 값에 따라 조절
            float volume = skillVolumeSlider.value / 100;
            skillSoundValue.text = Mathf.FloorToInt(skillVolumeSlider.value).ToString();
            AudioManager.Instance.SkillVolumeSetting(volume);
        }
        CheckChangeSetting();
    }
    public void OnInputSkillValueChanged()
    {
        // 텍스트 필드의 값을 읽어와 슬라이더의 값으로 설정
        float value = skillVolumeSlider.value;
        if (float.TryParse(skillSoundValue.text, out value))
        {
            value = Mathf.Clamp(value, 0f, volumeSlider.value);
            skillVolumeSlider.value = value;
        }
        CheckChangeSetting();
    }
    public void MuteUiSound()
    {
        if (totalSoundToggle.isOn)
            return;

        if (uiSoundToggle.isOn) // 토글이 켜져있다면 꺼지도록 설정
        {
            SetMuteUISound();
        }
        else // 토글이 꺼져있다면 켜지도록 설정
        {
            SetActiveUISound();
        }
        CheckChangeSetting();
    }

    void SetMuteUISound()
    {
        // 슬라이더가 회색으로 비활성화 된 이미지로 수정
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(80, 80, 80, 255);
        uiVolumeSlider.colors = colors;
        uiVolumeSliderHandle.color = new Color32(80, 80, 80, 255);

        // 사운드 0으로 설정
        AudioManager.Instance.UIVolumeSetting(0);
    }

    void SetActiveUISound()
    {
        // 슬라이더가 원래의 색으로 활성화 된 이미지로 수정
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(255, 255, 255, 255);
        uiVolumeSlider.colors = colors;
        uiVolumeSliderHandle.color = new Color32(255, 255, 255, 255);

        // Volume에 설정된 값으로 할당
        float volume = uiVolumeSlider.value / 100f;
        AudioManager.Instance.UIVolumeSetting(volume);
    }
    public void OnUIVolumeChanged()
    {
        if (uiVolumeSlider.value > volumeSlider.value) // Total Sound보다 작을때만 수정되도록 구현
        {
            uiVolumeSlider.value = volumeSlider.value;
            float volume = uiVolumeSlider.value / 100;
            uiSoundValue.text = Mathf.FloorToInt(uiVolumeSlider.value).ToString();
            AudioManager.Instance.UIVolumeSetting(volume);
        }
        else
        {
            // 음량을 슬라이더의 값에 따라 조절
            float volume = uiVolumeSlider.value / 100;
            uiSoundValue.text = Mathf.FloorToInt(uiVolumeSlider.value).ToString();
            AudioManager.Instance.UIVolumeSetting(volume);
        }
        CheckChangeSetting();
    }
    public void OnInputUIValueChanged()
    {
        // 텍스트 필드의 값을 읽어와 슬라이더의 값으로 설정
        float value = uiVolumeSlider.value;
        if (float.TryParse(uiSoundValue.text, out value))
        {
            value = Mathf.Clamp(value, 0f, volumeSlider.value);
            uiVolumeSlider.value = value;
        }
        CheckChangeSetting();
    }
    void OnSensitivityChanged()
    {
        if (isChanging) return; // 감도를 변경이 끝나면 값이 적용되도록 구현

        // 마우스 감도를 슬라이더의 값에 따라 조절
        isChanging = true;
        sensitivityValue.text = Mathf.FloorToInt(sensitivitySlider.value).ToString();
        isChanging = false;
    }

    public void OnSensitivityPointerUp()
    {
        // 슬라이더 바에서 클릭을 뗐을 때 감도 변경 적용
        ApplySensitivityChange();
    }

    // 마우스 감도를 조절하는 함수
    void ApplySensitivityChange()
    {       
        float value = sensitivitySlider.value / 100f;
        customCursor.isSetting = true;
        customCursor.magnification = Mathf.Max(value, 0.1f);
        customCursor.LoadSensitivity();
        CheckChangeSetting();
    }
    // 사용자가 텍스트 필드에 값을 입력할 때마다 호출되는 함수
    public void OnSensitivityInputValueChanged()
    {
        if (isChanging) return; 

        // 텍스트 필드의 값을 읽어와 슬라이더의 값으로 설정
        isChanging = true;
        float value = sensitivitySlider.value;
        if (float.TryParse(sensitivityValue.text, out value))
        {
            value = Mathf.Clamp(value, 0, 100f);
            sensitivitySlider.value = value;         
        }
        ApplySensitivityChange();
        isChanging = false;
    }
    // 설정한 정보를 저장하는 함수
    public void SaveSetting()
    {
        PlayerPrefs.SetInt("volumeValue", Mathf.FloorToInt(volumeSlider.value));
        PlayerPrefs.SetInt("bgmVolumeValue", Mathf.FloorToInt(bgmVolumeSlider.value));
        PlayerPrefs.SetInt("gameVolumeValue", Mathf.FloorToInt(gameVolumeSlider.value));
        PlayerPrefs.SetInt("skillVolumeValue", Mathf.FloorToInt(skillVolumeSlider.value));
        PlayerPrefs.SetInt("uiVolumeValue", Mathf.FloorToInt(uiVolumeSlider.value));
        PlayerPrefs.SetInt("cursorSensitivity", Mathf.FloorToInt(sensitivitySlider.value));
        PlayerPrefs.SetInt("isMuteTotalSound", totalSoundToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("isMuteBgmSound", bgmSoundToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("isMuteGameSound", gameSoundToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("isMuteSkillSound", skillSoundToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("isMuteUiSound", uiSoundToggle.isOn ? 1 : 0);

        foreach (var entry in KeySetting.keys)
        {
            PlayerPrefs.SetString(entry.Key.ToString(), entry.Value.ToString());
        }

        PlayerPrefs.Save();
        GameManager.Instance.isChangeSetting = false;
    }
    // 설정창을 끌때 원래의 세팅으로 되돌리는 함수
    public void CancleSetting()
    {
        volumeSlider.value = PlayerPrefs.GetInt("volumeValue", basicVolumeValue);
        bgmVolumeSlider.value = PlayerPrefs.GetInt("bgmVolumeValue", maxVolume);
        gameVolumeSlider.value = PlayerPrefs.GetInt("gameVolumeValue", maxVolume);
        skillVolumeSlider.value = PlayerPrefs.GetInt("skillVolumeValue", maxVolume);
        uiVolumeSlider.value = PlayerPrefs.GetInt("uiVolumeValue", maxVolume);
        sensitivitySlider.value = PlayerPrefs.GetInt("cursorSensitivity", basicSensitivityValue);
        totalSoundToggle.isOn = PlayerPrefs.GetInt("isMuteTotalSound", 0) == 1 ? true : false;
        bgmSoundToggle.isOn = PlayerPrefs.GetInt("isMuteBgmSound", 0) == 1 ? true : false;
        gameSoundToggle.isOn = PlayerPrefs.GetInt("isMuteGameSound", 0) == 1 ? true : false;
        skillSoundToggle.isOn = PlayerPrefs.GetInt("isMuteSkillSound", 0) == 1 ? true : false;
        uiSoundToggle.isOn = PlayerPrefs.GetInt("isMuteUiSound", 0) == 1 ? true : false;

        // 키세팅 복구 추가
        foreach (KeyAction keyAction in System.Enum.GetValues(typeof(KeyAction)))
        {
            if (keyAction == KeyAction.KEYCOUNT) continue;

            string keyString = PlayerPrefs.GetString(keyAction.ToString(), null);
            if (!string.IsNullOrEmpty(keyString))
            {
                KeyCode loadedKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyString);
                KeySetting.keys[keyAction] = loadedKey;
            }
            else
            {
                KeySetting.keys[keyAction] = defaultKeys[(int)keyAction];
            }
        }

        PlayerPrefs.Save();
        UpdateVolumeSliderMax();
        GameManager.Instance.isChangeSetting = false;
    }
    // 바뀐 값이 있는지 비교하는 함수
    void CheckChangeSetting()
    {
        // 저장된 값들
        int savedVolume = PlayerPrefs.GetInt("volumeValue", basicVolumeValue);
        int savedBgmVolume = PlayerPrefs.GetInt("bgmVolumeValue", maxVolume);
        int savedGameVolume = PlayerPrefs.GetInt("gameVolumeValue", maxVolume);
        int savedSkillVolume = PlayerPrefs.GetInt("skillVolumeValue", maxVolume);
        int savedUIVolume = PlayerPrefs.GetInt("uiVolumeValue", maxVolume);
        int savedSensitivity = PlayerPrefs.GetInt("cursorSensitivity", basicSensitivityValue);
        bool savedIsMuteTotalSound = PlayerPrefs.GetInt("isMuteTotalSound", 0) == 1 ? true : false;
        bool savedIsMuteBgmSound = PlayerPrefs.GetInt("isMuteBgmSound", 0) == 1 ? true : false;
        bool savedIsMuteGameSound = PlayerPrefs.GetInt("isMuteGameSound", 0) == 1 ? true : false;
        bool savedIsMuteSkillSound = PlayerPrefs.GetInt("isMuteSkillSound", 0) == 1 ? true : false;
        bool savedIsMuteUISound = PlayerPrefs.GetInt("isMuteUiSound", 0) == 1 ? true : false;

        // 현재 값들
        int curVolume = Mathf.FloorToInt(volumeSlider.value);
        int curBgmVolume = Mathf.FloorToInt(bgmVolumeSlider.value);
        int curGameVolume = Mathf.FloorToInt(gameVolumeSlider.value);
        int curSkillVolume = Mathf.FloorToInt(skillVolumeSlider.value);
        int curUIVolume = Mathf.FloorToInt(uiVolumeSlider.value);
        int curSensitivity = Mathf.FloorToInt(sensitivitySlider.value);
        bool curIsMuteTotalSound = totalSoundToggle.isOn;
        bool curIsMuteBgmSound = bgmSoundToggle.isOn;
        bool curIsMuteGameSound = gameSoundToggle.isOn;
        bool curIsMuteSkillSound = skillSoundToggle.isOn;
        bool curIsMuteUISound = uiSoundToggle.isOn;

        // 키세팅 비교 부분 추가
        foreach (KeyAction keyAction in System.Enum.GetValues(typeof(KeyAction)))
        {
            if (keyAction == KeyAction.KEYCOUNT) continue;

            KeyCode savedKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(keyAction.ToString(), defaultKeys[(int)keyAction].ToString()));
            KeyCode currentKey = KeySetting.keys[keyAction];

            if (savedKey != currentKey)
            {
                GameManager.Instance.isChangeSetting = true;
                return; // 변경 여부가 발견되면 즉시 리턴
            }
        }

        if (savedVolume != curVolume || savedSensitivity != curSensitivity || savedBgmVolume != curBgmVolume || savedGameVolume != curGameVolume || savedSkillVolume != curSkillVolume || savedUIVolume != curUIVolume || savedIsMuteTotalSound != curIsMuteTotalSound || savedIsMuteBgmSound != curIsMuteBgmSound || savedIsMuteGameSound != curIsMuteGameSound || savedIsMuteSkillSound != curIsMuteSkillSound || savedIsMuteUISound != curIsMuteUISound) // 현재 설정과 저장된 설정이 일치하는지 확인
        {
            GameManager.Instance.isChangeSetting = true; // isChangeSetting이 true일 경우 설정을 되돌릴건지 확인하는 UI 출력
        }
        else
        {
            GameManager.Instance.isChangeSetting = false;
        }
    }

    // bgm 효과음 등은 Total Sound의 값보다 높게 설정되지 않게 설정하는 함수
    void UpdateVolumeSliderMax()
    {
        float curVolume = volumeSlider.value;
        if (bgmVolumeSlider.value > curVolume)
        {
            bgmVolumeSlider.value = curVolume;
        }
        if (gameVolumeSlider.value > curVolume)
        {
            gameVolumeSlider.value = curVolume;
        }
        if (skillVolumeSlider.value > curVolume)
        {
            skillVolumeSlider.value = curVolume;
        }
        if (uiVolumeSlider.value > curVolume)
        {
            uiVolumeSlider.value = curVolume;
        }
    }

    void SetTotalToggle()
    {
        if (totalSoundToggle.isOn)
        {
            SetMuteTotalSound();
        }
        else
        {
            SetActiveTotalSound();
        }
    }
    void SetBgmToggle()
    {
        if (totalSoundToggle.isOn)
            return;

        if (bgmSoundToggle.isOn)
        {
            SetMuteBgmSound();
        }
        else
        {
            SetActiveBgmSound();
        }
    }
    void SetGameToggle()
    {
        if (totalSoundToggle.isOn)
            return;

        if (gameSoundToggle.isOn)
        {
            SetMuteGameSound();
        }
        else
        {
            SetActiveGameSound();
        }
    }
    void SetSkillToggle()
    {
        if (totalSoundToggle.isOn)
            return;

        if (skillSoundToggle.isOn)
        {
            SetMuteSkillSound();
        }
        else
        {
            SetActiveSkillSound();
        }
    }
    void SetUIToggle()
    {
        if (totalSoundToggle.isOn)
            return;

        if (uiSoundToggle.isOn)
        {
            SetMuteUISound();
        }
        else
        {
            SetActiveUISound();
        }
    }
}
