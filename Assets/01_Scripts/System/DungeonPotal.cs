using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonPotal : MonoBehaviour
{
    public GameObject dungeonList;
    public EscUI escUI;
    public Transform potalTransform;

    TilemapRenderer tileRenderer;
    bool isArrive;
    int playerLayer;

    private void Start()
    {
        tileRenderer = GetComponent<TilemapRenderer>();
        playerLayer = Player.Instance.render.sortingOrder;
    }
    // Update is called once per frame
    void Update()
    {
        DungeonUIOperation();
        AdjustLayer();
    }

    void DungeonUIOperation()
    {
        if (!isArrive)
        {
            dungeonList.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && dungeonList.activeSelf)
        {
            dungeonList.SetActive(false);
            StartCoroutine(EscUiNotActive());
        }

        if (isArrive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeySetting.keys[KeyAction.INTERACTION])) && !dungeonList.activeSelf)
        {
            escUI.notActive = true;
            dungeonList.SetActive(true);
        }
        else if (isArrive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeySetting.keys[KeyAction.INTERACTION])) && dungeonList.activeSelf)
        {
            dungeonList.SetActive(false);
            StartCoroutine(EscUiNotActive());
        }
    }

    void AdjustLayer()
    {
        if(Player.Instance.transform.position.y > potalTransform.position.y)
        {
            tileRenderer.sortingOrder = playerLayer + 1;
        }
        else
        {
            tileRenderer.sortingOrder = playerLayer - 1;
        }

    }
    IEnumerator EscUiNotActive()
    {
        yield return null;

        escUI.notActive = false;
    }

    public void HideDungeonList()
    {
        dungeonList.SetActive(false);
        StartCoroutine(EscUiNotActive());
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
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(EscUiNotActive());
            }
        }
    }
}
