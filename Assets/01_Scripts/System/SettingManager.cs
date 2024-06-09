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

    bool isChanging;

    int basicVolumeValue = 70;
    int basicSensitivityValue = 50;
    int maxVolume;

    void Start()
    {
        // ��ü ���� Value �� ����
        maxVolume = PlayerPrefs.HasKey("volumeValue") ? PlayerPrefs.GetInt("volumeValue") : basicVolumeValue;

        // �����̴��� On Value Changed�� �޼��� ���
        sensitivitySlider.onValueChanged.AddListener(delegate { OnSensitivityChanged(); });
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(); });
        bgmVolumeSlider.onValueChanged.AddListener(delegate { OnBGMVolumeChanged(); });
        gameVolumeSlider.onValueChanged.AddListener(delegate { OnGameVolumeChanged(); });
        skillVolumeSlider.onValueChanged.AddListener(delegate { OnSkillVolumeChanged(); });
        uiVolumeSlider.onValueChanged.AddListener(delegate { OnUIVolumeChanged(); });

        // �����̴��� InputField�� ����� �� or �⺻ �� �Ҵ�
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
 
        // Toggle �ʱ���� ����
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
        gameVolumeSlider.colors = colors;
        skillVolumeSlider.colors = colors;
        uiVolumeSlider.colors = colors;

        volumeSliderHandle.color = new Color32(80, 80, 80, 255);
        bgmVolumeSliderHandle.color = new Color32(80, 80, 80, 255);
        gameVolumeSliderHandle.color = new Color32(80, 80, 80, 255);
        skillVolumeSliderHandle.color = new Color32(80, 80, 80, 255);
        uiVolumeSliderHandle.color = new Color32(80, 80, 80, 255);

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

        // BGM�� ���Ұ� ���¸� ������� �ʰ� ����
        if(!bgmSoundToggle.isOn)
        {
            bgmVolumeSlider.colors = colors;
            bgmVolumeSliderHandle.color = new Color32(255, 255, 255, 255);
        }
        // Game ���尡 ���Ұ� ���¸� ������� �ʰ� ����
        if (!gameSoundToggle.isOn)
        {
            gameVolumeSlider.colors = colors;
            gameVolumeSliderHandle.color = new Color32(255, 255, 255, 255);
        }
        // Skill ���尡 ���Ұ� ���¸� ������� �ʰ� ����
        if (!skillSoundToggle.isOn)
        {
            skillVolumeSlider.colors = colors;
            skillVolumeSliderHandle.color = new Color32(255, 255, 255, 255);
        }
        // UI ���尡 ���Ұ� ���¸� ������� �ʰ� ����
        if (!uiSoundToggle.isOn)
        {
            uiVolumeSlider.colors = colors;
            uiVolumeSliderHandle.color = new Color32(255, 255, 255, 255);
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

        if (gameSoundToggle.isOn) // ����� �����ִٸ� �������� ����
        {
            SetMuteGameSound();
        }
        else // ����� �����ִٸ� �������� ����
        {
            SetActiveGameSound();
        }

        if (skillSoundToggle.isOn) // ����� �����ִٸ� �������� ����
        {
            SetMuteSkillSound();
        }
        else // ����� �����ִٸ� �������� ����
        {
            SetActiveSkillSound();
        }
        if (uiSoundToggle.isOn) // ����� �����ִٸ� �������� ����
        {
            SetMuteUISound();
        }
        else // ����� �����ִٸ� �������� ����
        {
            SetActiveUISound();
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
    public void MuteGameSound()
    {
        if (totalSoundToggle.isOn)
            return;

        if (gameSoundToggle.isOn) // ����� �����ִٸ� �������� ����
        {
            SetMuteGameSound();
        }
        else // ����� �����ִٸ� �������� ����
        {
            SetActiveGameSound();
        }
        CheckChangeSetting();
    }

    void SetMuteGameSound()
    {
        // �����̴��� ȸ������ ��Ȱ��ȭ �� �̹����� ����
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(80, 80, 80, 255);
        gameVolumeSlider.colors = colors;
        gameVolumeSliderHandle.color = new Color32(80, 80, 80, 255);

        // ���� 0���� ����
        AudioManager.Instance.GameVolumeSetting(0);
    }

    void SetActiveGameSound()
    {
        // �����̴��� ������ ������ Ȱ��ȭ �� �̹����� ����
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(255, 255, 255, 255);
        gameVolumeSlider.colors = colors;
        gameVolumeSliderHandle.color = new Color32(255, 255, 255, 255);

        // Volume�� ������ ������ �Ҵ�
        float volume = gameVolumeSlider.value / 100f;
        AudioManager.Instance.GameVolumeSetting(volume);
    }
    public void OnGameVolumeChanged()
    {
        if (gameVolumeSlider.value > volumeSlider.value) // Total Sound���� �������� �����ǵ��� ����
        {
            gameVolumeSlider.value = volumeSlider.value;
            float volume = gameVolumeSlider.value / 100;
            gameSoundValue.text = Mathf.FloorToInt(gameVolumeSlider.value).ToString();
            AudioManager.Instance.GameVolumeSetting(volume);
        }
        else
        {
            // ������ �����̴��� ���� ���� ����
            float volume = gameVolumeSlider.value / 100;
            gameSoundValue.text = Mathf.FloorToInt(gameVolumeSlider.value).ToString();
            AudioManager.Instance.GameVolumeSetting(volume);
        }
        CheckChangeSetting();
    }
    public void OnInputGameValueChanged()
    {
        // �ؽ�Ʈ �ʵ��� ���� �о�� �����̴��� ������ ����
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

        if (skillSoundToggle.isOn) // ����� �����ִٸ� �������� ����
        {
            SetMuteSkillSound();
        }
        else // ����� �����ִٸ� �������� ����
        {
            SetActiveSkillSound();
        }
        CheckChangeSetting();
    }

    void SetMuteSkillSound()
    {
        // �����̴��� ȸ������ ��Ȱ��ȭ �� �̹����� ����
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(80, 80, 80, 255);
        skillVolumeSlider.colors = colors;
        skillVolumeSliderHandle.color = new Color32(80, 80, 80, 255);

        // ���� 0���� ����
        AudioManager.Instance.SkillVolumeSetting(0);
    }

    void SetActiveSkillSound()
    {
        // �����̴��� ������ ������ Ȱ��ȭ �� �̹����� ����
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(255, 255, 255, 255);
        skillVolumeSlider.colors = colors;
        skillVolumeSliderHandle.color = new Color32(255, 255, 255, 255);

        // Volume�� ������ ������ �Ҵ�
        float volume = skillVolumeSlider.value / 100f;
        AudioManager.Instance.SkillVolumeSetting(volume);
    }
    public void OnSkillVolumeChanged()
    {
        if (skillVolumeSlider.value > volumeSlider.value) // Total Sound���� �������� �����ǵ��� ����
        {
            skillVolumeSlider.value = volumeSlider.value;
            float volume = skillVolumeSlider.value / 100;
            skillSoundValue.text = Mathf.FloorToInt(skillVolumeSlider.value).ToString();
            AudioManager.Instance.SkillVolumeSetting(volume);
        }
        else
        {
            // ������ �����̴��� ���� ���� ����
            float volume = skillVolumeSlider.value / 100;
            skillSoundValue.text = Mathf.FloorToInt(skillVolumeSlider.value).ToString();
            AudioManager.Instance.SkillVolumeSetting(volume);
        }
        CheckChangeSetting();
    }
    public void OnInputSkillValueChanged()
    {
        // �ؽ�Ʈ �ʵ��� ���� �о�� �����̴��� ������ ����
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

        if (uiSoundToggle.isOn) // ����� �����ִٸ� �������� ����
        {
            SetMuteUISound();
        }
        else // ����� �����ִٸ� �������� ����
        {
            SetActiveUISound();
        }
        CheckChangeSetting();
    }

    void SetMuteUISound()
    {
        // �����̴��� ȸ������ ��Ȱ��ȭ �� �̹����� ����
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(80, 80, 80, 255);
        uiVolumeSlider.colors = colors;
        uiVolumeSliderHandle.color = new Color32(80, 80, 80, 255);

        // ���� 0���� ����
        AudioManager.Instance.UIVolumeSetting(0);
    }

    void SetActiveUISound()
    {
        // �����̴��� ������ ������ Ȱ��ȭ �� �̹����� ����
        ColorBlock colors = volumeSlider.colors;
        colors.normalColor = new Color32(255, 255, 255, 255);
        uiVolumeSlider.colors = colors;
        uiVolumeSliderHandle.color = new Color32(255, 255, 255, 255);

        // Volume�� ������ ������ �Ҵ�
        float volume = uiVolumeSlider.value / 100f;
        AudioManager.Instance.UIVolumeSetting(volume);
    }
    public void OnUIVolumeChanged()
    {
        if (uiVolumeSlider.value > volumeSlider.value) // Total Sound���� �������� �����ǵ��� ����
        {
            uiVolumeSlider.value = volumeSlider.value;
            float volume = uiVolumeSlider.value / 100;
            uiSoundValue.text = Mathf.FloorToInt(uiVolumeSlider.value).ToString();
            AudioManager.Instance.UIVolumeSetting(volume);
        }
        else
        {
            // ������ �����̴��� ���� ���� ����
            float volume = uiVolumeSlider.value / 100;
            uiSoundValue.text = Mathf.FloorToInt(uiVolumeSlider.value).ToString();
            AudioManager.Instance.UIVolumeSetting(volume);
        }
        CheckChangeSetting();
    }
    public void OnInputUIValueChanged()
    {
        // �ؽ�Ʈ �ʵ��� ���� �о�� �����̴��� ������ ����
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
        PlayerPrefs.SetInt("gameVolumeValue", Mathf.FloorToInt(gameVolumeSlider.value));
        PlayerPrefs.SetInt("skillVolumeValue", Mathf.FloorToInt(skillVolumeSlider.value));
        PlayerPrefs.SetInt("uiVolumeValue", Mathf.FloorToInt(uiVolumeSlider.value));
        PlayerPrefs.SetInt("cursorSensitivity", Mathf.FloorToInt(sensitivitySlider.value));
        PlayerPrefs.SetInt("isMuteTotalSound", totalSoundToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("isMuteBgmSound", bgmSoundToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("isMuteGameSound", gameSoundToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("isMuteSkillSound", skillSoundToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("isMuteUiSound", uiSoundToggle.isOn ? 1 : 0);

        GameManager.Instance.isChangeSetting = false;
    }
    // ����â�� ���� ������ �������� �ǵ����� �Լ�
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

        UpdateVolumeSliderMax();
        GameManager.Instance.isChangeSetting = false;
    }
    // �ٲ� ���� �ִ��� ���ϴ� �Լ�
    void CheckChangeSetting()
    {
        // ����� ����
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

        // ���� ����
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

        if (savedVolume != curVolume || savedSensitivity != curSensitivity || savedBgmVolume != curBgmVolume || savedGameVolume != curGameVolume || savedSkillVolume != curSkillVolume || savedUIVolume != curUIVolume || savedIsMuteTotalSound != curIsMuteTotalSound || savedIsMuteBgmSound != curIsMuteBgmSound || savedIsMuteGameSound != curIsMuteGameSound || savedIsMuteSkillSound != curIsMuteSkillSound || savedIsMuteUISound != curIsMuteUISound) // ���� ������ ����� ������ ��ġ�ϴ��� Ȯ��
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
