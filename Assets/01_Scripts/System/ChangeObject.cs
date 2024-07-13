using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeObject : MonoBehaviour
{
    public Transform standard;

    TilemapRenderer tileRenderer;
    int playerLayer;
    private void Start()
    {
        tileRenderer = GetComponent<TilemapRenderer>();
        playerLayer = Player.Instance.render.sortingOrder;
    }
    void Update()
    {
        AdjustLayer();
    }

    void AdjustLayer()
    {
        if (Player.Instance.transform.position.y > standard.position.y)
        {
            tileRenderer.sortingOrder = playerLayer + 1;
        }
        else
        {
            tileRenderer.sortingOrder = playerLayer - 1;
        }

    }
}
