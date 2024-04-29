using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SheetsReader : MonoBehaviour
{
    //https://docs.google.com/spreadsheets/d/1KtBPiux5aSs811uTH3fL7JUiktPNm9xxYhYAmo6KRLk/edit?usp=sharing
    private const string sheetLink = "https://docs.google.com/spreadsheets/d/";
    private const string sheetId = "1KtBPiux5aSs811uTH3fL7JUiktPNm9xxYhYAmo6KRLk";
    private const string endLink = "/export?format=tsv&gid=";
    private const string upgradeGID = "469223592";
    private const string costGID = "1780920375";
    private const string destroyGID = "136100522";

    public List<int[]> upgradeTable = new List<int[]>();
    public List<int[]> costTable = new List<int[]>();
    public List<int[]> destroyTable = new List<int[]>();
    void Start()
    {
        StartCoroutine(GetSheetsCoroutine(upgradeGID, upgradeTable));
        StartCoroutine(GetSheetsCoroutine(costGID, costTable));
        StartCoroutine(GetSheetsCoroutine(destroyGID, destroyTable));
    }

    IEnumerator GetSheetsCoroutine(string gid, List<int[]> table)
    {
        UnityWebRequest req = UnityWebRequest.Get(string.Concat(sheetLink, sheetId, endLink, gid));
        yield return req.SendWebRequest();
        if (req.isDone)
        {
            string sheetData = req.downloadHandler.text;
            table = ParseSheet(sheetData);
        }        
    }

    public List<int[]> ParseSheet(string sheetData)
    {
        List<int[]> sheetTable = new List<int[]>();
        string[] rows = sheetData.Split('\n');
        int colCount = rows[0].Split('\t').Length;
        for (int i = 0; i < rows.Length; i++)
        {
            int[] intTable = Array.ConvertAll(rows[i].Split('\t'), int.Parse);
            sheetTable.Add(intTable);
        }
        return sheetTable;
    }
}
