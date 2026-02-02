using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int collectedCash;
    public int collectedTears;
    public int accumCollectedTears; //제출한 실적 눈물
    //public DateTime hiredDate;
    public string hiredDate;

    public Dictionary<int, string> deepTalkChoices = new();
    public Dictionary<string, bool> specialRecipes = new();
}