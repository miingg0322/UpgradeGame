using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notice : MonoBehaviour
{
    public GameObject noticeUi;
    public Image noticeIcon;
    public Text noticeText;   

    WaitForSecondsRealtime wait;

    private void Awake()
    {
        wait = new WaitForSecondsRealtime(5);
    }

    public IEnumerator NoticeRoutine()
    {
        noticeUi.SetActive(true);

        yield return wait;

        noticeUi.SetActive(false);
    }
}
