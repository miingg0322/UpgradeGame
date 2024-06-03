using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider bgmVolumeSlider;
    public Slider sensitivitySlider;

    public Image volumeSliderHandle;
    public Image bgmVolumeSliderHandle;

    public TMP_InputField soundValue;
    public TMP_InputField bgmSoundValue;
    public TMP_InputField sensitivityValue;

    public Toggle totalSoundToggle;
    public Toggle bgmSoundToggle;

    public CustomCursor customCursor;

    bool isChanging;

    int basicVolumeValue = 70;
    int basicSensitivityValue = 50;
    int maxVolume;

    void Start()
    {
        // 전체 사운드 Value 값 설정
        maxVolume = PlayerPrefs.HasKey("volumeValue") ? PlayerPrefs.GetInt("volumeValue") : basicVolumeValue;

        // 슬라이더의 On Value Changed에 메서드 등록
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(); });
        bgmVolumeSlider.onValueChanged.AddListener(delegate { OnBGMVolumeChanged(); });
        sensitivitySlider.onValueChanged.AddListener(delegate { OnSensitivityChanged(); });

        // 슬라이더와 InputField에 저장된 값 or 기본 값 할당
        volumeSlider.value = maxVolume;
        soundValue.text = maxVolume.ToString();
        bgmVolumeSlider.value = PlayerPrefs.HasKey("bgmVolumeValue") ? Mathf.Min(PlayerPrefs.GetInt("bgmVolumeValue"), maxVolume) : maxVolume;
        bgmSoundValue.text = bgmVolumeSlider.value.ToString();
        sensitivitySlider.value = PlayerPrefs.HasKey("cursorSensitivity") ? PlayerPrefs.GetInt("cursorSensitivity") : basicSensitivityValue;
        sensitivityValue.text = sensitivitySlider.value.ToString();
 
        // Toggle 초기상태 설정
        totalSoundToggle.isOn = PlayerPrefs.HasKey("isMuteTotalSound") ? (PlayerPrefs.GetInt("isMuteTotalSound") == 1 ? true : false) : false;
        bgmSoundToggle.isOn = PlayerPrefs.HasKey("isMuteBgmSound") ? (PlayerPrefs.GetInt("isMuteBgmSound") == 1 ? true : false) : false;

        SetTotalToggle();
        SetBgmToggle();
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
        Debug.Log(totalSoundToggle.isOn);
        CheckChangeSetting();
    }

    void SetMuteTotalSound()
    {
        // 슬라이더가 회색으로 비활성화 된 이미지로 수정
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(80, 80, 80, 255);
        volumeSlider.colors = colors;
        bgmVolumeSlider.colors = colors;
        volumeSliderHandle.color = new Color32(80, 80, 80, 255);
        bgmVolumeSliderHandle.color = new Color32(80, 80, 80, 255);

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

        // bgm이 음소거 상태면 실행되지 않게 구현
        if(!bgmSoundToggle.isOn)
        {
            bgmVolumeSlider.colors = colors;
            bgmVolumeSliderHandle.color = new Color32(255, 255, 255, 255);
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
        PlayerPrefs.SetInt("cursorSensitivity", Mathf.FloorToInt(sensitivitySlider.value));
        PlayerPrefs.SetInt("isMuteTotalSound", totalSoundToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("isMuteBgmSound", bgmSoundToggle.isOn ? 1 : 0);

        GameManager.Instance.isChangeSetting = false;
    }
    // 설정창을 끌때 원래의 세팅으로 되돌리는 함수
    public void CancleSetting()
    {
        volumeSlider.value = PlayerPrefs.GetInt("volumeValue", basicVolumeValue);
        bgmVolumeSlider.value = PlayerPrefs.GetInt("bgmVolumeValue", maxVolume);
        sensitivitySlider.value = PlayerPrefs.GetInt("cursorSensitivity", basicSensitivityValue);
        totalSoundToggle.isOn = PlayerPrefs.GetInt("isMuteTotalSound", 0) == 1 ? true : false;
        bgmSoundToggle.isOn = PlayerPrefs.GetInt("isMuteBgmSound", 0) == 1 ? true : false;

        UpdateVolumeSliderMax();
        GameManager.Instance.isChangeSetting = false;
    }
    // 바뀐 값이 있는지 비교하는 함수
    void CheckChangeSetting()
    {
        // 저장된 값들
        int savedVolume = PlayerPrefs.GetInt("volumeValue", basicVolumeValue);
        int savedBgmVolume = PlayerPrefs.GetInt("bgmVolumeValue", maxVolume);
        int savedSensitivity = PlayerPrefs.GetInt("cursorSensitivity", basicSensitivityValue);
        bool savedIsMuteTotalSound = PlayerPrefs.GetInt("isMuteTotalSound", 0) == 1 ? true : false;
        bool savedIsMuteBgmSound = PlayerPrefs.GetInt("isMuteBgmSound", 0) == 1 ? true : false;

        // 현재 값들
        int curVolume = Mathf.FloorToInt(volumeSlider.value);
        int curBgmVolume = Mathf.FloorToInt(bgmVolumeSlider.value);
        int curSensitivity = Mathf.FloorToInt(sensitivitySlider.value);
        bool curIsMuteTotalSound = totalSoundToggle.isOn;
        bool curIsMuteBgmSound = bgmSoundToggle.isOn;

        if (savedVolume != curVolume || savedSensitivity != curSensitivity || savedBgmVolume != curBgmVolume 
            || savedIsMuteTotalSound != curIsMuteTotalSound || savedIsMuteBgmSound != curIsMuteBgmSound) // 현재 설정과 저장된 설정이 일치하는지 확인
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
}
