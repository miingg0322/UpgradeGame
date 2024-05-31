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
    bool isMuteBgmSound;

    int basicVolumeValue = 70;
    int basicSensitivityValue = 50;
    int maxVolume;

    void Start()
    {
        maxVolume = PlayerPrefs.HasKey("volumeValue") ? PlayerPrefs.GetInt("volumeValue") : basicVolumeValue;

        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(); });
        bgmVolumeSlider.onValueChanged.AddListener(delegate { OnBGMVolumeChanged(); });
        sensitivitySlider.onValueChanged.AddListener(delegate { OnSensitivityChanged(); });

        volumeSlider.value = maxVolume;
        soundValue.text = maxVolume.ToString();
        bgmVolumeSlider.value = PlayerPrefs.HasKey("bgmVolumeValue") ? Mathf.Min(PlayerPrefs.GetInt("bgmVolumeValue"), maxVolume) : maxVolume;
        bgmSoundValue.text = bgmVolumeSlider.value.ToString();
        sensitivitySlider.value = PlayerPrefs.HasKey("cursorSensitivity") ? PlayerPrefs.GetInt("cursorSensitivity") : basicSensitivityValue;
        sensitivityValue.text = sensitivitySlider.value.ToString();
 
        totalSoundToggle.isOn = PlayerPrefs.HasKey("isMuteTotalSound") ? (PlayerPrefs.GetInt("isMuteTotalSound") == 1 ? true : false) : false;

        if (totalSoundToggle.isOn)
        {
            SetMuteTotalSound();
        }
        else
        {
            SetActiveTotalSound();
        }
    }
    void OnVolumeChanged()
    {
        // 음량을 슬라이더의 값에 따라 조절
        float volume = volumeSlider.value / 100f;
        soundValue.text = Mathf.FloorToInt(volumeSlider.value).ToString();
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
        if (float.TryParse(soundValue.text, out value))
        {
            value = Mathf.Clamp(value, 0f, 100f);
            volumeSlider.value = value;
        }
        UpdateVolumeSliderMax();
        CheckChangeSetting();
    }

    public void MuteTotalSound()
    {
        if (totalSoundToggle.isOn)
        {
            SetMuteTotalSound();
        }
        else
        {
            SetActiveTotalSound();          
        }
        Debug.Log(totalSoundToggle.isOn);
        CheckChangeSetting();
    }

    void SetMuteTotalSound()
    {
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(80, 80, 80, 255);
        volumeSlider.colors = colors;
        bgmVolumeSlider.colors = colors;
        volumeSliderHandle.color = new Color32(80, 80, 80, 255);
        bgmVolumeSliderHandle.color = new Color32(80, 80, 80, 255);

        AudioListener.volume = 0f;
    }

    void SetActiveTotalSound()
    {
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(255, 255, 255, 255);
        volumeSlider.colors = colors;
        bgmVolumeSlider.colors = colors;
        volumeSliderHandle.color = new Color32(255, 255, 255, 255);
        bgmVolumeSliderHandle.color = new Color32(255, 255, 255, 255);


        float volume = volumeSlider.value / 100f;
        AudioListener.volume = volume;
    }

    void OnBGMVolumeChanged()
    {
        if (bgmVolumeSlider.value > volumeSlider.value)
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
        if (isChanging) return;

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
    public void SaveSetting()
    {
        PlayerPrefs.SetInt("volumeValue", Mathf.FloorToInt(volumeSlider.value));
        PlayerPrefs.SetInt("bgmVolumeValue", Mathf.FloorToInt(bgmVolumeSlider.value));
        PlayerPrefs.SetInt("cursorSensitivity", Mathf.FloorToInt(sensitivitySlider.value));
        PlayerPrefs.SetInt("isMuteTotalSound", totalSoundToggle.isOn ? 1 : 0);

        GameManager.Instance.isChangeSetting = false;
    }
    public void CancleSetting()
    {
        volumeSlider.value = PlayerPrefs.GetInt("volumeValue", basicVolumeValue);
        bgmVolumeSlider.value = PlayerPrefs.GetInt("bgmVolumeValue", maxVolume);
        sensitivitySlider.value = PlayerPrefs.GetInt("cursorSensitivity", basicSensitivityValue);
        totalSoundToggle.isOn = PlayerPrefs.GetInt("isMuteTotalSound", 0) == 1 ? true : false;

        UpdateVolumeSliderMax();
        GameManager.Instance.isChangeSetting = false;
    }

    void CheckChangeSetting()
    {
        int savedVolume = PlayerPrefs.GetInt("volumeValue", basicVolumeValue);
        int savedBgmVolume = PlayerPrefs.GetInt("bgmVolumeValue", maxVolume);
        int savedSensitivity = PlayerPrefs.GetInt("cursorSensitivity", basicSensitivityValue);
        bool savedIsMuteTotalSound = PlayerPrefs.GetInt("isMuteTotalSound", 0) == 1 ? true : false;

        int curVolume = Mathf.FloorToInt(volumeSlider.value);
        int curBgmVolume = Mathf.FloorToInt(bgmVolumeSlider.value);
        int curSensitivity = Mathf.FloorToInt(sensitivitySlider.value);
        bool curIsMuteTotalSound = totalSoundToggle.isOn;

        if (savedVolume != curVolume || savedSensitivity != curSensitivity || savedBgmVolume != curBgmVolume 
            || savedIsMuteTotalSound != curIsMuteTotalSound)
        {
            GameManager.Instance.isChangeSetting = true;
        }
        else
        {
            GameManager.Instance.isChangeSetting = false;
        }
    }

    void UpdateVolumeSliderMax()
    {
        float curVolume = volumeSlider.value;
        if (bgmVolumeSlider.value > curVolume)
        {
            bgmVolumeSlider.value = curVolume;
        }
    }
}
