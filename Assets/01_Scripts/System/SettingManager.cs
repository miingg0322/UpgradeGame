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
        // ��ü ���� Value �� ����
        maxVolume = PlayerPrefs.HasKey("volumeValue") ? PlayerPrefs.GetInt("volumeValue") : basicVolumeValue;

        // �����̴��� On Value Changed�� �޼��� ���
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(); });
        bgmVolumeSlider.onValueChanged.AddListener(delegate { OnBGMVolumeChanged(); });
        sensitivitySlider.onValueChanged.AddListener(delegate { OnSensitivityChanged(); });

        // �����̴��� InputField�� ����� �� or �⺻ �� �Ҵ�
        volumeSlider.value = maxVolume;
        soundValue.text = maxVolume.ToString();
        bgmVolumeSlider.value = PlayerPrefs.HasKey("bgmVolumeValue") ? Mathf.Min(PlayerPrefs.GetInt("bgmVolumeValue"), maxVolume) : maxVolume;
        bgmSoundValue.text = bgmVolumeSlider.value.ToString();
        sensitivitySlider.value = PlayerPrefs.HasKey("cursorSensitivity") ? PlayerPrefs.GetInt("cursorSensitivity") : basicSensitivityValue;
        sensitivityValue.text = sensitivitySlider.value.ToString();
 
        // Toggle �ʱ���� ����
        totalSoundToggle.isOn = PlayerPrefs.HasKey("isMuteTotalSound") ? (PlayerPrefs.GetInt("isMuteTotalSound") == 1 ? true : false) : false;
        bgmSoundToggle.isOn = PlayerPrefs.HasKey("isMuteBgmSound") ? (PlayerPrefs.GetInt("isMuteBgmSound") == 1 ? true : false) : false;

        SetTotalToggle();
        SetBgmToggle();
    }
    void OnVolumeChanged()
    {
        // ������ �����̴��� ���� ���� ����
        float volume = volumeSlider.value / 100f;
        soundValue.text = Mathf.FloorToInt(volumeSlider.value).ToString();

        // ���Ұ� �����϶��� ��ü ���尡 0�� �ǰ� ����
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

    // ����ڰ� �ؽ�Ʈ �ʵ忡 ���� �Է��� ������ ȣ��Ǵ� �Լ�
    public void OnInputValueChanged()
    {
        // �ؽ�Ʈ �ʵ��� ���� �о�� �����̴��� ������ ����
        float value = volumeSlider.value;
        if (float.TryParse(soundValue.text, out value)) // �Էµ� ���� float ���̿��� ����
        {
            value = Mathf.Clamp(value, 0f, 100f);
            volumeSlider.value = value;
        }
        UpdateVolumeSliderMax();
        CheckChangeSetting();
    }

    // Total Sound�� ���Ұ� Toggle�� ������ ȣ��Ǵ� �Լ�
    public void MuteTotalSound()
    {
        if (totalSoundToggle.isOn) // ����� �����ִٸ� �������� ����
        {
            SetMuteTotalSound();
        }
        else // ����� �����ִٸ� �������� ����
        {
            SetActiveTotalSound();          
        }
        Debug.Log(totalSoundToggle.isOn);
        CheckChangeSetting();
    }

    void SetMuteTotalSound()
    {
        // �����̴��� ȸ������ ��Ȱ��ȭ �� �̹����� ����
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(80, 80, 80, 255);
        volumeSlider.colors = colors;
        bgmVolumeSlider.colors = colors;
        volumeSliderHandle.color = new Color32(80, 80, 80, 255);
        bgmVolumeSliderHandle.color = new Color32(80, 80, 80, 255);

        // ���� 0���� ����
        AudioListener.volume = 0f;
    }

    void SetActiveTotalSound()
    {
        // �����̴��� ������ ������ Ȱ��ȭ �� �̹����� ����
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(255, 255, 255, 255);
        volumeSlider.colors = colors;     
        volumeSliderHandle.color = new Color32(255, 255, 255, 255);

        // bgm�� ���Ұ� ���¸� ������� �ʰ� ����
        if(!bgmSoundToggle.isOn)
        {
            bgmVolumeSlider.colors = colors;
            bgmVolumeSliderHandle.color = new Color32(255, 255, 255, 255);
        }
        
        // Volume�� ������ ������ �Ҵ�
        float volume = volumeSlider.value / 100f;
        AudioListener.volume = volume;
    }

    // Total Sound�� ���Ұ� Toggle�� ������ ȣ��Ǵ� �Լ�
    public void MuteBgmSound()
    {
        if (totalSoundToggle.isOn)
            return;

        if (bgmSoundToggle.isOn) // ����� �����ִٸ� �������� ����
        {
            SetMuteBgmSound();
        }
        else // ����� �����ִٸ� �������� ����
        {
            SetActiveBgmSound();
        }
        CheckChangeSetting();
    }

    void SetMuteBgmSound()
    {
        // �����̴��� ȸ������ ��Ȱ��ȭ �� �̹����� ����
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(80, 80, 80, 255);
        bgmVolumeSlider.colors = colors;
        bgmVolumeSliderHandle.color = new Color32(80, 80, 80, 255);

        // ���� 0���� ����
        AudioManager.Instance.BGMVolumeSetting(0);
    }

    void SetActiveBgmSound()
    {
        // �����̴��� ������ ������ Ȱ��ȭ �� �̹����� ����
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(255, 255, 255, 255);
        bgmVolumeSlider.colors = colors;
        bgmVolumeSliderHandle.color = new Color32(255, 255, 255, 255);

        // Volume�� ������ ������ �Ҵ�
        float volume = bgmVolumeSlider.value / 100f;
        AudioManager.Instance.BGMVolumeSetting(volume);
    }

    void OnBGMVolumeChanged()
    {
        if (bgmVolumeSlider.value > volumeSlider.value) // Total Sound���� �������� �����ǵ��� ����
        {
            bgmVolumeSlider.value = volumeSlider.value;
            float volume = bgmVolumeSlider.value / 100;
            bgmSoundValue.text = Mathf.FloorToInt(bgmVolumeSlider.value).ToString();
            AudioManager.Instance.BGMVolumeSetting(volume);
        }
        else
        {
            // ������ �����̴��� ���� ���� ����
            float volume = bgmVolumeSlider.value / 100;
            bgmSoundValue.text = Mathf.FloorToInt(bgmVolumeSlider.value).ToString();
            AudioManager.Instance.BGMVolumeSetting(volume);
        }
        CheckChangeSetting();
    }

    // ����ڰ� �ؽ�Ʈ �ʵ忡 ���� �Է��� ������ ȣ��Ǵ� �Լ�
    public void OnInputBGMValueChanged()
    {
        // �ؽ�Ʈ �ʵ��� ���� �о�� �����̴��� ������ ����
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
        if (isChanging) return; // ������ ������ ������ ���� ����ǵ��� ����

        // ���콺 ������ �����̴��� ���� ���� ����
        isChanging = true;
        sensitivityValue.text = Mathf.FloorToInt(sensitivitySlider.value).ToString();
        isChanging = false;
    }

    public void OnSensitivityPointerUp()
    {
        // �����̴� �ٿ��� Ŭ���� ���� �� ���� ���� ����
        ApplySensitivityChange();
    }

    // ���콺 ������ �����ϴ� �Լ�
    void ApplySensitivityChange()
    {       
        float value = sensitivitySlider.value / 100f;
        customCursor.isSetting = true;
        customCursor.magnification = Mathf.Max(value, 0.1f);
        customCursor.LoadSensitivity();
        CheckChangeSetting();
    }
    // ����ڰ� �ؽ�Ʈ �ʵ忡 ���� �Է��� ������ ȣ��Ǵ� �Լ�
    public void OnSensitivityInputValueChanged()
    {
        if (isChanging) return; 

        // �ؽ�Ʈ �ʵ��� ���� �о�� �����̴��� ������ ����
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
    // ������ ������ �����ϴ� �Լ�
    public void SaveSetting()
    {
        PlayerPrefs.SetInt("volumeValue", Mathf.FloorToInt(volumeSlider.value));
        PlayerPrefs.SetInt("bgmVolumeValue", Mathf.FloorToInt(bgmVolumeSlider.value));
        PlayerPrefs.SetInt("cursorSensitivity", Mathf.FloorToInt(sensitivitySlider.value));
        PlayerPrefs.SetInt("isMuteTotalSound", totalSoundToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("isMuteBgmSound", bgmSoundToggle.isOn ? 1 : 0);

        GameManager.Instance.isChangeSetting = false;
    }
    // ����â�� ���� ������ �������� �ǵ����� �Լ�
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
    // �ٲ� ���� �ִ��� ���ϴ� �Լ�
    void CheckChangeSetting()
    {
        // ����� ����
        int savedVolume = PlayerPrefs.GetInt("volumeValue", basicVolumeValue);
        int savedBgmVolume = PlayerPrefs.GetInt("bgmVolumeValue", maxVolume);
        int savedSensitivity = PlayerPrefs.GetInt("cursorSensitivity", basicSensitivityValue);
        bool savedIsMuteTotalSound = PlayerPrefs.GetInt("isMuteTotalSound", 0) == 1 ? true : false;
        bool savedIsMuteBgmSound = PlayerPrefs.GetInt("isMuteBgmSound", 0) == 1 ? true : false;

        // ���� ����
        int curVolume = Mathf.FloorToInt(volumeSlider.value);
        int curBgmVolume = Mathf.FloorToInt(bgmVolumeSlider.value);
        int curSensitivity = Mathf.FloorToInt(sensitivitySlider.value);
        bool curIsMuteTotalSound = totalSoundToggle.isOn;
        bool curIsMuteBgmSound = bgmSoundToggle.isOn;

        if (savedVolume != curVolume || savedSensitivity != curSensitivity || savedBgmVolume != curBgmVolume 
            || savedIsMuteTotalSound != curIsMuteTotalSound || savedIsMuteBgmSound != curIsMuteBgmSound) // ���� ������ ����� ������ ��ġ�ϴ��� Ȯ��
        {
            GameManager.Instance.isChangeSetting = true; // isChangeSetting�� true�� ��� ������ �ǵ������� Ȯ���ϴ� UI ���
        }
        else
        {
            GameManager.Instance.isChangeSetting = false;
        }
    }

    // bgm ȿ���� ���� Total Sound�� ������ ���� �������� �ʰ� �����ϴ� �Լ�
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
