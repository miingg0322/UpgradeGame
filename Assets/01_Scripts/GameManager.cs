using Rito;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    public PoolManager pool;
    public Player player;
    public RandomItem ranItem;

    public int DungeonLevel;

    /// <summary>
    /// Key = Upgrade, Cost, Destroy
    /// </summary>
    public Dictionary<string, List<int[]>> dataTables = new Dictionary<string, List<int[]>>();
    public int probBase = 1000;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
