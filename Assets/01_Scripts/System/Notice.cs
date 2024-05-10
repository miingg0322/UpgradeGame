using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Notice : MonoBehaviour
{
    public GameObject noticeUi;
    public GameObject exitUi;
    public Image noticeIcon;
    public Text noticeText;   

    WaitForSecondsRealtime wait;

    private void Awake()
    {
        wait = new WaitForSecondsRealtime(5);
        GameManager.Instance.AssignNotice(this);
    }

    public IEnumerator NoticeRoutine()
    {
        noticeUi.SetActive(true);

        yield return wait;

        noticeUi.SetActive(false);
    }

    public void ActiveExitFarming()
    {
        exitUi.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CancleExitFarming()
    {
        exitUi.SetActive(false);
        Time.timeScale = 1f;
    }
}
