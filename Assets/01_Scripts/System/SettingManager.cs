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
        // ������ �����̴��� ���� ���� ����
        float volume = volumeSlider.value / 100f;
        soundValue.text = Mathf.FloorToInt(volumeSlider.value).ToString();
        AudioListener.volume = volume;
    }

    // ����ڰ� �ؽ�Ʈ �ʵ忡 ���� �Է��� ������ ȣ��Ǵ� �Լ�
    public void OnInputValueChanged()
    {
        // �ؽ�Ʈ �ʵ��� ���� �о�� �����̴��� ������ ����
        float value = volumeSlider.value;
        if (float.TryParse(soundValue.text, out value))
        {
            value = Mathf.Clamp(value, 0f, 100f);
            volumeSlider.value = value;
        }
    }
}
