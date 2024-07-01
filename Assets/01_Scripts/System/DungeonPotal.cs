using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPotal : MonoBehaviour
{
    public GameObject dungeonList;
    public EscUI escUI;

    bool isArrive;

    // Update is called once per frame
    void Update()
    {
        if (!isArrive)
        {
            dungeonList.SetActive(false);          
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && dungeonList.activeSelf)
        {         
            dungeonList.SetActive(false);
            StartCoroutine(EscUiNotActive());
        }

        if(isArrive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && !dungeonList.activeSelf)
        {
            escUI.notActive = true;
            dungeonList.SetActive(true);
        }
        else if (isArrive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && dungeonList.activeSelf)
        {
            dungeonList.SetActive(false);
            StartCoroutine(EscUiNotActive());
        }
    }

    IEnumerator EscUiNotActive()
    {
        yield return null;

        escUI.notActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isArrive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isArrive = false;
            StartCoroutine(EscUiNotActive());
        }
    }
}
