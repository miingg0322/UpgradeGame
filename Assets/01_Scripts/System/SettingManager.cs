using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public Slider volumeSlider;
    public TMP_InputField soundValue;

    public int basicValue = 70;

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value = basicValue;
        soundValue.text = basicValue.ToString();
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(); });       
    }

    void OnVolumeChanged()
    {
        // 음량을 슬라이더의 값에 따라 조절
        float volume = volumeSlider.value / 100f;
        soundValue.text = Mathf.FloorToInt(volumeSlider.value).ToString();
        AudioListener.volume = volume;
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
    }
}
