using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class SheetsReader : MonoBehaviour
{
    //https://docs.google.com/spreadsheets/d/1KtBPiux5aSs811uTH3fL7JUiktPNm9xxYhYAmo6KRLk/edit?usp=sharing
    private const string sheetLink = "https://docs.google.com/spreadsheets/d/";
    private const string sheetId = "1KtBPiux5aSs811uTH3fL7JUiktPNm9xxYhYAmo6KRLk";
    private const string endLink = "/export?format=tsv&gid=";
    [SerializeField]
    private const string upgradeGID = "469223592";
    [SerializeField]
    private const string costGID = "1780920375";
    [SerializeField]
    private const string destroyGID = "136100522";

    public readonly string[] gids = { upgradeGID, costGID, destroyGID };

    public string sheetData = "";

    void Awake()
    {
        StartCoroutine(SheetCo());
    }
    IEnumerator GetSheetsCoroutine(string gid)
    {
        UnityWebRequest req = UnityWebRequest.Get(string.Concat(sheetLink, sheetId, endLink, gid));
        yield return req.SendWebRequest();
        yield return new WaitUntil(()=>req.isDone);
        sheetData = req.downloadHandler.text;     
    }

    IEnumerator SheetCo()
    {
        for (int i = 0; i < gids.Length; i++)
        {
            string key = "";
            switch (i)
            {
                case 0:
                    key = "Upgrade";
                    break;
                case 1:
                    key = "Cost";
                    break;
                case 2:
                    key = "Destroy";
                    break;
                default:
                    break;
            }
            yield return StartCoroutine(GetSheetsCoroutine(gids[i]));
            GameManager.Instance.dataTables.Add(key, ParseSheet(sheetData));

        }

    }
    public List<int[]> ParseSheet(string sheetData)
    {
        //Debug.Log(sheetData);
        List<int[]> sheetTable = new List<int[]>();
        string[] rows = sheetData.Split('\n');
        //Debug.Log(rows[0]);
        //Debug.Log(rows.Length);
        int colCount = rows[0].Split('\t').Length;
        for (int i = 0; i < rows.Length; i++)
        {
            int[] intTable = Array.ConvertAll(rows[i].Split('\t'), int.Parse);
            //Debug.Log(intTable.Length);
            sheetTable.Add(intTable);
        }
        return sheetTable;
    }
}
