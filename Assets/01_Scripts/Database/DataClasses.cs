using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UserSchema
{
    ID = 1, PW, Nickname, Value
}
public enum CharSchema
{
    SLOT = 2, JOB, CLEAR, TUTORIAL = 6, SCORE, UPGRADE
}
[Serializable]
public class SheetResponse
{
    string order;
    public bool result;
    public string msg;
    public string[] data;
    public int count;

    public string[][] ParseData()
    {
        int len = data.Length / count;
        string[][] result = new string[count][];
        for (int i = 0; i < count; i++)
        {
            result[i] = new string[len];
        }

        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < len; j++)
            {
                result[i][j] = data[i * len + j];
            }
        }
        return result;
    }
}

public class UserData
{
    public string id;
    public string nickname;
    public UserData(string[] data)
    {
        id = data[(int)UserSchema.ID - 1];
        nickname = data[(int)UserSchema.Nickname - 1];
    }
}
public class CharacterData
{
    public int slot;
    public int job;
    public int clear;
    public string created;
    public bool tutorial;
    public int score;
    public int upgrade;

    public CharacterData(int slot, int job, int clear, string created, int tutorial, int score, int upgrade)
    {
        this.slot = slot;
        this.job = job;
        this.clear = clear;
        this.created = created;
        if (tutorial == 1)
            this.tutorial = true;
        else
            this.tutorial = false;
        this.score = score;
        this.upgrade = upgrade;
    }
    public CharacterData(string[] data)
    {
        slot = int.Parse(data[1]);
        job = int.Parse(data[2]);
        clear = int.Parse(data[3]);
        created = data[4];
        if (int.Parse(data[5]) == 1)
            tutorial = true;
        else
            tutorial = false;
        score = int.Parse(data[6]);
        upgrade = int.Parse(data[7]);
    }
}
