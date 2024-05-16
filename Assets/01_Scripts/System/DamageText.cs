using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public float destroyTime = 1f;

    TextMeshProUGUI text;
    Animator anim;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        anim = GetComponentInParent<Animator>();
    }

    public void Damaged(float damage)
    {
        int damageValue = Mathf.FloorToInt(damage);
        text.text = damageValue.ToString();
        anim.SetTrigger("Damage");

        Invoke("DestroyObject", destroyTime);
    }

    private void DestroyObject()
    {
        gameObject.SetActive(false);
    }
}
