using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WithdrawText : MonoBehaviour
{
    public Text descText;
    void Start()
    {
        descText.text = GameManager.Instance.userId + "/삭제한다"; 
    }
}
