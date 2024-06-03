using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterClass
{
    Class1, Class2, Class3
}
public class Character
{
    public CharacterClass charClass = CharacterClass.Class1;
    public int clear = 0;
    public string created;
    public int score;
    public int slot;
    public bool isDeleted;

    public Character(int charClass, int slot)
    {
        this.charClass = (CharacterClass)charClass;
        this.clear = 0;
        this.created = DateTime.Now.ToString();
        this.score = 0;
        this.slot = slot;
        this.isDeleted = false;
        Debug.Log(created);
    }
}
