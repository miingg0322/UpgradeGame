using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WithdrawText : MonoBehaviour
{
    public TMP_InputField descText;
    void Start()
    {
        descText.text = GameManager.Instance.userId + "/삭제한다"; 
    }
}
